using MongoDB.Driver;
using TaskList.Core.Dtos.UserDtos;
using TaskList.Core.Mappers;
using TaskList.Core.Repositories;
using TaskList.Core.Result;

namespace TaskList.Core.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponseDto>> GetUserByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await _userRepository.GetUserByIdAsync(id, ct);

        if (user == null)
        {
            return Result<UserResponseDto>
                .Failure("User was not found", ResultErrorType.NotFound);
        }

        var userResponseDto = user.UserToResponse();

        return Result<UserResponseDto>.Success(userResponseDto);
    }

    public async Task<Result<UserResponseDto>> CreateUserAsync(CreateUserDto userDto, CancellationToken ct)
    {
        try
        {
            var user = userDto.CreateDtoToUser();

            var createdUser = await _userRepository.CreateUserAsync(user, ct);

            var userResponse = createdUser.UserToResponse();

            return Result<UserResponseDto>.Success(userResponse);
        }
        catch (MongoWriteException ex) when (ex.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return Result<UserResponseDto>.Failure(
                "User with the same first and last name already exists",
                ResultErrorType.InvalidOperation
            );
        }
    }
}
