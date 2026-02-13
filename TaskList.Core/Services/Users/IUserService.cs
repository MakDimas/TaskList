using TaskList.Core.Dtos.UserDtos;
using TaskList.Core.Results;

namespace TaskList.Core.Services.Users;

public interface IUserService
{
    public Task<Result<UserResponseDto>> GetUserByIdAsync(Guid id, CancellationToken ct);
    public Task<Result<UserResponseDto>> CreateUserAsync(CreateUserDto userDto, CancellationToken ct);
}
