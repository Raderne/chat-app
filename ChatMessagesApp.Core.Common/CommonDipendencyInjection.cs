using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChatMessagesApp.Core.Common;

public static class CommonDipendencyInjection
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        return services;
    }
}
