namespace TaskList.Domain.TaskLists;

public class TaskList
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Guid> SharedWithUserIds { get; set; } = [];
}
