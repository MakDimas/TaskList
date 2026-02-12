using MongoDB.Driver;
using TaskList.Domain.Users;
using Tasks = TaskList.Domain.TaskLists.TaskList; 

namespace TaskList.Infrastructure.DataAccess;

public interface IMongoContext
{
    IMongoCollection<Tasks> TaskLists { get; }
    IMongoCollection<User> Users { get; }
}
