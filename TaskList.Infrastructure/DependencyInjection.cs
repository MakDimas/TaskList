using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskList.Core.Repositories;
using TaskList.Infrastructure.DataAccess;
using TaskList.Infrastructure.DataAccess.Repositories;
using TaskList.Infrastructure.DataAccess.Settings;

namespace TaskList.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(
            configuration.GetSection("MongoSettings"));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddScoped<IMongoContext, MongoContext>();

        services.AddScoped<ITaskListRepository, TaskListRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
