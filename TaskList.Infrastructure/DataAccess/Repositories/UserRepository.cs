using MongoDB.Driver;
using TaskList.Core.Repositories;
using TaskList.Domain.Users;

namespace TaskList.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoContext _mongoContext;

    public UserRepository(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task<User> GetUserByIdAsync(Guid id, CancellationToken ct)
    {
        var cursor = await _mongoContext.Users.FindAsync(u => u.Id == id, cancellationToken: ct);
        return await cursor.FirstOrDefaultAsync(ct);
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken ct)
    {
        await _mongoContext.Users.InsertOneAsync(user, new InsertOneOptions(), ct);
        return user;
    }
}
