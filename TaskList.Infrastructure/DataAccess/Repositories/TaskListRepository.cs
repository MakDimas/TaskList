using MongoDB.Driver;
using TaskList.Core.Dtos.Models;
using TaskList.Core.Dtos.TaskListDtos;
using TaskList.Core.Repositories;
using TaskList.Domain.Users;
using Tasks = TaskList.Domain.TaskLists.TaskList;

namespace TaskList.Infrastructure.DataAccess.Repositories;

public class TaskListRepository : ITaskListRepository
{
    private readonly IMongoContext _mongoContext;

    public TaskListRepository(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task<Tasks> CreateTaskListAsync(Tasks taskList, CancellationToken ct)
    {
        await _mongoContext.TaskLists
            .InsertOneAsync(taskList, new InsertOneOptions(), ct);

        return taskList;
    }

    public async Task DeleteTaskListAsync(Guid taskListId, CancellationToken ct)
    {
        await _mongoContext.TaskLists
            .DeleteOneAsync(tl => tl.Id == taskListId, ct);
    }

    public async Task<Tasks> GetTaskListByIdAsync(Guid taskListId, CancellationToken ct)
    {
        var cursor = await _mongoContext.TaskLists
            .FindAsync(tl => tl.Id == taskListId, cancellationToken: ct);

        return await cursor.FirstOrDefaultAsync(ct);
    }

    public async Task<List<Tasks>> GetTaskListsAsync(
        Guid userId, QueryParameters queryParams, CancellationToken ct)
    {
        var pageNumber = queryParams.PageNumber < 1 ? 1 : queryParams.PageNumber;
        var pageSize = queryParams.PageSize < 1 ? 10 : queryParams.PageSize;

        var filter = Builders<Tasks>.Filter.Or(
            Builders<Tasks>.Filter.Eq(t => t.OwnerId, userId),
            Builders<Tasks>.Filter.AnyEq(t => t.SharedWithUserIds, userId)
        );

        var find = _mongoContext.TaskLists
            .Find(filter);

        find = queryParams.SortByDateDirection?.ToLowerInvariant() switch
        {
            "asc" => find.SortBy(t => t.CreatedAtUTC),
            "desc" => find.SortByDescending(t => t.CreatedAtUTC),

            _ => find.SortByDescending(t => t.CreatedAtUTC)
        };

        var taskLists = await find
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(ct);

        return taskLists;
    }

    public async Task<(bool Exists, bool HasAccess, List<User> Users)> GetTaskListUserLinksAsync(
        TaskListOwnerModel queryModel, CancellationToken ct)
    {
        var taskList = await _mongoContext.TaskLists
        .Find(tl => tl.Id == queryModel.TaskListId)
        .Project(tl => new { tl.OwnerId, tl.SharedWithUserIds })
        .FirstOrDefaultAsync(ct);

        if (taskList == null)
            return (false, false, new List<User>());

        var hasAccess = taskList.OwnerId == queryModel.CurrentUserId ||
                        (taskList.SharedWithUserIds?.Contains(queryModel.CurrentUserId) ?? false);

        if (!hasAccess)
            return (true, false, new List<User>());

        var userIds = new List<Guid> { taskList.OwnerId };
        if (taskList.SharedWithUserIds != null)
            userIds.AddRange(taskList.SharedWithUserIds);

        var users = await _mongoContext.Users
            .Find(u => userIds.Contains(u.Id))
            .ToListAsync(ct);

        return (true, true, users);
    }

    public async Task<(bool Exists, bool HasAccess)> ModifyTaskListSharedUsersAsync(
        ModifyTaskListSharedUsersDto linkDto, bool isLinkOperation, CancellationToken ct)
    {
        var filter = Builders<Tasks>.Filter.And(
            Builders<Tasks>.Filter.Eq(tl => tl.Id, linkDto.QueryModel.TaskListId),
            Builders<Tasks>.Filter.Or(
                Builders<Tasks>.Filter.Eq(tl => tl.OwnerId, linkDto.QueryModel.CurrentUserId),
                Builders<Tasks>.Filter.AnyEq(tl => tl.SharedWithUserIds, linkDto.QueryModel.CurrentUserId)
            )
        );

        var update = isLinkOperation
            ? Builders<Tasks>.Update
                .AddToSet(x => x.SharedWithUserIds, linkDto.UserId)
            : Builders<Tasks>.Update
                .Pull(x => x.SharedWithUserIds, linkDto.UserId);

        var options = new FindOneAndUpdateOptions<Tasks>
        {
            ReturnDocument = ReturnDocument.After
        };

        var updatedTaskList = await _mongoContext.TaskLists
            .FindOneAndUpdateAsync(filter, update, options, ct);

        if (updatedTaskList != null)
        {
            return (true, true);
        }

        var exists = await _mongoContext.TaskLists
            .Find(tl => tl.Id == linkDto.QueryModel.TaskListId)
            .Project(tl => tl.Id)
            .AnyAsync(ct);

        return exists 
            ? (true, false)
            : (false, false);
    }

    public async Task<Tasks> UpdateTaskListAsync(UpdateTaskListDto updateDto, CancellationToken ct)
    {
        var filter = Builders<Tasks>.Filter.And(
            Builders<Tasks>.Filter.Eq(x => x.Id, updateDto.QueryModel.TaskListId),
            Builders<Tasks>.Filter.Or(
                Builders<Tasks>.Filter.Eq(x => x.OwnerId, updateDto.QueryModel.CurrentUserId),
                Builders<Tasks>.Filter.AnyEq(x => x.SharedWithUserIds, updateDto.QueryModel.CurrentUserId)
            )
        );

        var update = Builders<Tasks>.Update
            .Set(x => x.Name, updateDto.NewName);

        var options = new FindOneAndUpdateOptions<Tasks>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await _mongoContext.TaskLists
            .FindOneAndUpdateAsync(filter, update, options, ct);
    }
}
