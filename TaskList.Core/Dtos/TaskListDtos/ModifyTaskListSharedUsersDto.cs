using TaskList.Core.Dtos.Models;

namespace TaskList.Core.Dtos.TaskListDtos;

public class ModifyTaskListSharedUsersDto
{
    public TaskListOwnerModel QueryModel { get; set; }
    public Guid UserId { get; set; }
}
