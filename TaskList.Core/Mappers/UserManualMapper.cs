using TaskList.Core.Dtos.UserDtos;
using TaskList.Domain.Users;

namespace TaskList.Core.Mappers;

public static class UserManualMapper
{
    public static User CreateDtoToUser(this CreateUserDto userDto) =>
        new User
        {
            Id = Guid.NewGuid(),
            FirstName = userDto.FirstName,
            LastName = userDto.LastName
        };

    public static UserResponseDto UserToResponse(this User user) =>
        new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };
}
