using TaskList.Core.Dtos.Models;
using TaskList.Core.Dtos.TaskListDtos;
using TaskList.Core.Dtos.UserDtos;
using TaskList.Core.Mappers;
using TaskList.Core.Repositories;
using TaskList.Core.Results;

namespace TaskList.Core.Services.TaskLists;

public class TaskListService : ITaskListService
{
    private readonly ITaskListRepository _taskListRepository;
    private readonly IUserRepository _userRepository;

    public TaskListService(ITaskListRepository taskListrepository, IUserRepository userRepository)
    {
        _taskListRepository = taskListrepository;
        _userRepository = userRepository;
    }

    public async Task<Result<TaskListResponseDto>> CreateTaskListAsync(CreateTaskListDto taskListDto, CancellationToken ct)
    {
        var userExist = await _userRepository
            .GetUserByIdAsync(taskListDto.OwnerId, ct) != null;

        if (!userExist)
        {
            return Result<TaskListResponseDto>.Failure(
                $"User with id: {taskListDto.OwnerId} was not found. Can`t create task list",
                ResultErrorType.NotFound);
        }

        var taskListToCreate = taskListDto.CreateDtoToTaskList();

        var createdTaskList = await _taskListRepository
            .CreateTaskListAsync(taskListToCreate, ct);

        var taskListResponseDto = createdTaskList.TaskListToResponse();

        return Result<TaskListResponseDto>.Success(taskListResponseDto);
    }

    public async Task<Result<TaskListResponseDto>> GetTaskListByIdAsync(TaskListOwnerModel queryModel, CancellationToken ct)
    {
        var taskList = await _taskListRepository
            .GetTaskListByIdAsync(queryModel.TaskListId, ct);

        if (taskList == null)
        {
            return Result<TaskListResponseDto>.Failure(
                $"Task list with id: {queryModel.TaskListId} was not found", 
                ResultErrorType.NotFound);
        }

        if (taskList.OwnerId != queryModel.CurrentUserId && 
            !taskList.SharedWithUserIds.Contains(queryModel.CurrentUserId))
        {
            return Result<TaskListResponseDto>.Failure(
                $"Current user doesn't have access to this resource",
                ResultErrorType.Forbidden);
        }

        var taskListResponse = taskList.TaskListToResponse();

        return Result<TaskListResponseDto>.Success(taskListResponse);
    }

    public async Task<Result> DeleteTaskListAsync(TaskListOwnerModel queryModel, CancellationToken ct)
    {
        var taskList = await _taskListRepository
            .GetTaskListByIdAsync(queryModel.TaskListId, ct);

        if (taskList == null)
        {
            return Result.Failure(
                $"Task list with id: {queryModel.TaskListId} was not found",
                ResultErrorType.NotFound);
        }

        if (taskList.OwnerId != queryModel.CurrentUserId)
        {
            return Result.Failure(
                $"Current user doesn't have access to this resource",
                ResultErrorType.Forbidden);
        }

        await _taskListRepository
            .DeleteTaskListAsync(queryModel.TaskListId, ct);

        return Result.Success();
    }

    public async Task<Result<PaginationResult<TaskListResponseDto>>> GetTaskListsAsync(Guid userId, QueryParameters queryParams, CancellationToken ct)
    {
        var taskLists = await _taskListRepository
            .GetTaskListsAsync(userId, queryParams, ct);

        if (taskLists.Count < 1)
        {
            return Result<PaginationResult<TaskListResponseDto>>
                .Failure(
                    $"Task lists was not found",
                    ResultErrorType.NotFound);
        }

        var taskListsResult = taskLists.TaskListsToResponses();

        return Result<PaginationResult<TaskListResponseDto>>.Success(new PaginationResult<TaskListResponseDto>
        {
            Items = taskListsResult,
            TotalCount = taskListsResult.Count
        });
    }

    public async Task<Result<TaskListResponseDto>> UpdateTasklistAsync(UpdateTaskListDto updateDto, CancellationToken ct)
    {
        var updated = await _taskListRepository.UpdateTaskListAsync(updateDto, ct);

        if (updated != null)
        {
            return Result<TaskListResponseDto>.Success(updated.TaskListToResponse());
        }

        var exists = await _taskListRepository.GetTaskListByIdAsync(updateDto.QueryModel.TaskListId, ct);
        if (exists == null)
        {
            return Result<TaskListResponseDto>.Failure(
                $"Task list with id {updateDto.QueryModel.TaskListId} was not found",
                ResultErrorType.NotFound);
        }

        return Result<TaskListResponseDto>.Failure(
            "Current user doesn't have access to this resource",
            ResultErrorType.Forbidden);
    }

    public async Task<Result<List<UserResponseDto>>> GetTaskListUserLinksAsync(TaskListOwnerModel queryModel, CancellationToken ct)
    {
        var linkedUsers = await _taskListRepository.GetTaskListUserLinksAsync(queryModel, ct);

        if (!linkedUsers.Exists)
            return Result<List<UserResponseDto>>.Failure(
                $"Task list with id {queryModel.TaskListId} was not found",
                ResultErrorType.NotFound);

        if (!linkedUsers.HasAccess)
            return Result<List<UserResponseDto>>.Failure(
                "Current user doesn't have access to this resource",
                ResultErrorType.Forbidden);

        return Result<List<UserResponseDto>>.Success(linkedUsers.Users.UsersToResponses());
    }

    public async Task<Result<string>> ModifyTaskListSharedUsersAsync(ModifyTaskListSharedUsersDto linkDto, bool isLinkOperation, CancellationToken ct)
    {
        var (Exists, HasAccess) = await _taskListRepository.ModifyTaskListSharedUsersAsync(linkDto, isLinkOperation, ct);

        if (!Exists)
            return Result<string>.Failure(
                $"Task list with id {linkDto.QueryModel.TaskListId} was not found",
                ResultErrorType.NotFound);

        if (!HasAccess)
            return Result<string>.Failure(
                "Current user doesn't have access to this resource",
                ResultErrorType.Forbidden);

        return Result<string>.Success($"User with id: {linkDto.UserId} was {(isLinkOperation? "linked" : "unlinked")} successfully");
    }
}
