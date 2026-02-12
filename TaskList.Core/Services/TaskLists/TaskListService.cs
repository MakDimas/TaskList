using TaskList.Core.Dtos.TaskListDtos;
using TaskList.Core.Mappers;
using TaskList.Core.Repositories;
using TaskList.Core.Result;

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
        var userExist = await _userRepository.GetUserByIdAsync(taskListDto.OwnerId, ct) != null;

        if (!userExist)
        {
            return Result<TaskListResponseDto>.Failure(
                $"User with id: {taskListDto.OwnerId} was not found. Can`t create task list",
                ResultErrorType.InvalidOperation);
        }

        var taskListToCreate = taskListDto.CreateDtoToTaskList();

        var createdTaskList = await _taskListRepository.CreateTaskListAsync(taskListToCreate, ct);

        var taskListResponseDto = createdTaskList.TaskListToResponse();

        return Result<TaskListResponseDto>.Success(taskListResponseDto);
    }
}
