using Microsoft.AspNetCore.Mvc;
using TaskList.Core.Dtos.UserDtos;
using TaskList.Core.Services.Users;

namespace TaskList.WebApi.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken ct)
    {
        var getResult = await _userService.GetUserByIdAsync(userId, ct);

        if (getResult.IsFailure)
            return BadRequest(getResult.Error);

        return Ok(getResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto userDto, CancellationToken ct)
    {
        var createResult = await _userService.CreateUserAsync(userDto, ct);

        if (createResult.IsFailure)
            return BadRequest(createResult.Error);

        return Ok(createResult.Value);
    }
}
