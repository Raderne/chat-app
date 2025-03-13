using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Infrastructure.Context;
using ChatMessagesApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatMessagesApp.Infrastructure;

public static class InfrastructureDipendencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(typeof(ApplicationContext).Name);
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(connectionString)
        );

        services.AddScoped<IContext, ApplicationContext>();
        services.AddScoped<IDomainEventService, DomainEventService>();
        services.AddScoped<IMessageService, MessageService>();

        return services;
    }
}
