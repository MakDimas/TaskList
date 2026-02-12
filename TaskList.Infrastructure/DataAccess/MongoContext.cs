using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskList.Infrastructure.DataAccess.Settings;
using Tasks = TaskList.Domain.TaskLists.TaskList;
using TaskList.Domain.Users;

namespace TaskList.Infrastructure.DataAccess;

public class MongoContext : IMongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IMongoClient mongoClient,
        IOptions<MongoSettings> mongoSettings)
    {
        _database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        CreateIndexes();
    }

    public IMongoCollection<Tasks> TaskLists =>
        _database.GetCollection<Tasks>("TaskLists");

    public IMongoCollection<User> Users =>
        _database.GetCollection<User>("Users");

    private void CreateIndexes()
    {
        var users = Users;

        var indexKeys = Builders<User>.IndexKeys
            .Ascending(u => u.FirstName)
            .Ascending(u => u.LastName);

        var indexOptions = new CreateIndexOptions
        {
            Unique = true
        };

        var indexModel = new CreateIndexModel<User>(indexKeys, indexOptions);

        users.Indexes.CreateOne(indexModel);
    }
}
