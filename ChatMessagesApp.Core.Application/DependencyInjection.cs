using ChatMessagesApp.Core.Application.Helpers;
using ChatMessagesApp.Core.Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ChatMessagesApp.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddSingleton<IUserConnectionManager, UserConnectionManager>();

        return services;
    }
}
