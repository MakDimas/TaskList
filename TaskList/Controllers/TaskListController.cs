using Microsoft.AspNetCore.Mvc;
using TaskList.Core.Dtos.TaskListDtos;
using TaskList.Core.Services.TaskLists;

namespace TaskList.WebApi.Controllers;

public class TaskListController : BaseController
{
    private readonly ITaskListService _taskListService;

    public TaskListController(ITaskListService taskListService)
    {
        _taskListService = taskListService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTaskList([FromBody] CreateTaskListDto taskListDto, CancellationToken ct)
    {
        var createdResult = await _taskListService.CreateTaskListAsync(taskListDto, ct);

        if (createdResult.IsFailure)
            return BadRequest(createdResult.Error);

        return Ok(createdResult.Value);
    }
}
