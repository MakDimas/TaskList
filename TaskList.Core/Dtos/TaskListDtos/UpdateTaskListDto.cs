using TaskList.Core.Dtos.Models;

namespace TaskList.Core.Dtos.TaskListDtos;

public class UpdateTaskListDto
{
    public TaskListOwnerModel QueryModel { get; set; }
    public string NewName { get; set; }
}
