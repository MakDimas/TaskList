using TaskList.Core.Dtos.Models;
using TaskList.Core.Dtos.TaskListDtos;
using TaskList.Core.Dtos.UserDtos;
using TaskList.Core.Results;

namespace TaskList.Core.Services.TaskLists;

public interface ITaskListService
{
    public Task<Result<TaskListResponseDto>> CreateTaskListAsync(CreateTaskListDto taskListDto, CancellationToken ct);
    public Task<Result<TaskListResponseDto>> GetTaskListByIdAsync(TaskListOwnerModel queryModel, CancellationToken ct);
    public Task<Result> DeleteTaskListAsync(TaskListOwnerModel queryModel, CancellationToken ct);
    public Task<Result<PaginationResult<TaskListResponseDto>>> GetTaskListsAsync(Guid userId, QueryParameters queryParams, CancellationToken ct);
    public Task<Result<TaskListResponseDto>> UpdateTasklistAsync(UpdateTaskListDto updateDto, CancellationToken ct);
    public Task<Result<List<UserResponseDto>>> GetTaskListUserLinksAsync(TaskListOwnerModel queryModel, CancellationToken ct);
    public Task<Result<string>> ModifyTaskListSharedUsersAsync(ModifyTaskListSharedUsersDto linkDto, bool isLinkOperation, CancellationToken ct);
}
