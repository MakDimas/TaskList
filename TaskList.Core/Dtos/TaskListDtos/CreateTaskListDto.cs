using System.ComponentModel.DataAnnotations;

namespace TaskList.Core.Dtos.TaskListDtos;

public class CreateTaskListDto
{
    public string Name { get; set; }

    public Guid OwnerId { get; set; }
}
