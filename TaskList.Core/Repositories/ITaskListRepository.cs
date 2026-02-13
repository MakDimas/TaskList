using TaskList.Core.Dtos.Models;
using TaskList.Core.Dtos.TaskListDtos;
using TaskList.Domain.Users;
using Tasks = TaskList.Domain.TaskLists.TaskList;

namespace TaskList.Core.Repositories;

public interface ITaskListRepository
{
    public Task<Tasks> CreateTaskListAsync(Tasks taskList, CancellationToken ct);
    public Task<Tasks> UpdateTaskListAsync(UpdateTaskListDto updateDto, CancellationToken ct);
    public Task DeleteTaskListAsync(Guid taskListId, CancellationToken ct);
    public Task<Tasks> GetTaskListByIdAsync(Guid taskListId, CancellationToken ct);
    public Task<List<Tasks>> GetTaskListsAsync(Guid userId, QueryParameters queryParams, CancellationToken ct);
    public Task<(bool Exists, bool HasAccess)> ModifyTaskListSharedUsersAsync(ModifyTaskListSharedUsersDto linkDto, bool isLinkOperation, CancellationToken ct);
    public Task<(bool Exists, bool HasAccess, List<User> Users)> GetTaskListUserLinksAsync(TaskListOwnerModel queryModel, CancellationToken ct);
}
