namespace TaskList.Core.Dtos.Models;

public class TaskListOwnerModel
{
    public Guid CurrentUserId { get; set; }
    public Guid TaskListId { get; set; }
}
