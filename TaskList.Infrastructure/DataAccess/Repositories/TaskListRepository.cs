using MongoDB.Driver;
using TaskList.Core.Repositories;
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
        await _mongoContext.TaskLists.InsertOneAsync(taskList, new InsertOneOptions(), ct);
        return taskList;
    }

    public Task DeleteTaskListAsync()
    {
        throw new NotImplementedException();
    }

    public Task GetTaskListAsync()
    {
        throw new NotImplementedException();
    }

    public Task GetTaskListsAsync()
    {
        throw new NotImplementedException();
    }

    public Task GetTaskListUserLinksAsync()
    {
        throw new NotImplementedException();
    }

    public Task LinkTaskListToUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task RemoveLinkedUserFromTaskListAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateTaskListAsync()
    {
        throw new NotImplementedException();
    }
}
