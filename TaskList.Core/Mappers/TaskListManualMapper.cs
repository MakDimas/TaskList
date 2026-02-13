using TaskList.Core.Dtos.TaskListDtos;
using Tasks = TaskList.Domain.TaskLists.TaskList;

namespace TaskList.Core.Mappers;

public static class TaskListManualMapper
{
    public static TaskListResponseDto TaskListToResponse(this Tasks taskList) =>
        new TaskListResponseDto
        {
            Id = taskList.Id,
            Name = taskList.Name,
            CreatedAtUTC = taskList.CreatedAtUTC,
            OwnerId = taskList.OwnerId,
            SharedWithUserIds = taskList.SharedWithUserIds,
        };

    public static Tasks CreateDtoToTaskList(this CreateTaskListDto createDto) =>
        new Tasks
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            CreatedAtUTC = DateTime.Now,
            OwnerId = createDto.OwnerId,
        };

    public static List<TaskListResponseDto> TaskListsToResponses(this IEnumerable<Tasks> taskLists) =>
        taskLists.Select(tl => tl.TaskListToResponse()).ToList();
}
