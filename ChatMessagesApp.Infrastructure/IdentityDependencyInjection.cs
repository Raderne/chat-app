using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Infrastructure.Context;
using ChatMessagesApp.Infrastructure.Identity;
using ChatMessagesApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatMessagesApp.Infrastructure;

public static class IdentityDependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(typeof(IdentityContext).Name);
        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IIdentityContext>(provider => provider.GetService<IdentityContext>());

        services.AddTransient<IIdentityService, IdentityService>();

        services.AddIdentity<AppUser, ApplicationRole>(opt =>
        {
            opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
        })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddSignInManager();

        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme =
            opts.DefaultChallengeScheme =
            opts.DefaultForbidScheme =
            opts.DefaultScheme =
            opts.DefaultSignInScheme =
            opts.DefaultSignOutScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };

                opts.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["token"];

                        Console.WriteLine($"Access token: {accessToken}");

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                            var token = new JwtSecurityToken(accessToken);
                            var claims = new ClaimsIdentity(token.Claims);
                            context.HttpContext.User = new ClaimsPrincipal(claims);
                        }
                        else
                        {
                            Console.WriteLine("No access token found in query string.");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();


        return services;
    }
}
