using Tasks = TaskList.Domain.TaskLists.TaskList;

namespace TaskList.Core.Repositories;

public interface ITaskListRepository
{
    public Task<Tasks> CreateTaskListAsync(Tasks taskList, CancellationToken ct);
    public Task UpdateTaskListAsync();
    public Task DeleteTaskListAsync();
    public Task GetTaskListAsync();
    public Task GetTaskListsAsync();
    public Task LinkTaskListToUserAsync();
    public Task GetTaskListUserLinksAsync();
    public Task RemoveLinkedUserFromTaskListAsync();
}
