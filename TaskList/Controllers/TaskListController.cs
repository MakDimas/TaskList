using Microsoft.AspNetCore.Mvc;
using TaskList.Core.Dtos.Models;
using TaskList.Core.Dtos.TaskListDtos;
using TaskList.Core.Results;
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

        return createdResult.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetTaskList([FromQuery] TaskListOwnerModel queryModel, CancellationToken ct)
    {
        var getResult = await _taskListService.GetTaskListByIdAsync(queryModel, ct);

        return getResult.ToActionResult();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTaskList([FromQuery] TaskListOwnerModel queryModel, CancellationToken ct)
    {
        var deleteResult = await _taskListService.DeleteTaskListAsync(queryModel, ct);

        return deleteResult.ToActionResult();
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetTaskLists(Guid userId, [FromQuery] QueryParameters queryParams, CancellationToken ct)
    {
        var getResult = await _taskListService.GetTaskListsAsync(userId, queryParams, ct);

        return getResult.ToActionResult();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateTaskList([FromBody] UpdateTaskListDto updateDto, CancellationToken ct)
    {
        var updateResult = await _taskListService.UpdateTasklistAsync(updateDto, ct);

        return updateResult.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetTaskListUserLinks([FromQuery] TaskListOwnerModel queryModel, CancellationToken ct)
    {
        var getResult = await _taskListService.GetTaskListUserLinksAsync(queryModel, ct);

        return getResult.ToActionResult();
    }

    [HttpPatch]
    public async Task<IActionResult> LinkTaskListToUser([FromBody] ModifyTaskListSharedUsersDto linkDto, CancellationToken ct)
    {
        var modifyResult = await _taskListService.ModifyTaskListSharedUsersAsync(linkDto, true, ct);

        return modifyResult.ToActionResult();
    }

    [HttpPatch]
    public async Task<IActionResult> RemoveLinkedUserFromTaskListAsync([FromBody] ModifyTaskListSharedUsersDto linkDto, CancellationToken ct)
    {
        var modifyResult = await _taskListService.ModifyTaskListSharedUsersAsync(linkDto, false, ct);

        return modifyResult.ToActionResult();
    }
}
