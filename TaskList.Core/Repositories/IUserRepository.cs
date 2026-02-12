using TaskList.Domain.Users;

namespace TaskList.Core.Repositories;

public interface IUserRepository
{
    public Task<User> GetUserByIdAsync(Guid id, CancellationToken ct);
    public Task<User> CreateUserAsync(User user, CancellationToken ct);
}
