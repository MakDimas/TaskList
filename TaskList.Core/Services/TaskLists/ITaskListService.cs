using TaskList.Core.Dtos.TaskListDtos;
using TaskList.Core.Result;

namespace TaskList.Core.Services.TaskLists;

public interface ITaskListService
{
    public Task<Result<TaskListResponseDto>> CreateTaskListAsync(CreateTaskListDto taskListDto, CancellationToken ct);
}
