using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskList.Core.Services.TaskLists;
using TaskList.Core.Services.Users;

namespace TaskList.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationCore(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ITaskListService, TaskListService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}