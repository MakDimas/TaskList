namespace TaskList.Core.Dtos.TaskListDtos;

public class TaskListResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAtUTC { get; set; }
    public Guid OwnerId { get; set; }
    public List<Guid> SharedWithUserIds { get; set; }
}
