using ChatMessagesApp.Core.Application;
using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Common;
using ChatMessagesApp.Infrastructure;
using ChatMessagesApp.Infrastructure.Services;
using ChatMessagesApp.Web.FrontOffice.Hubs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddCommon();
builder.Services.AddApplication();

builder.Services.AddScoped<INotificationService, SignalRNotificationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.AddSecurityDefinition(
        "BearerAuth",
        new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "Bearer" }
    );

    cfg.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "BearerAuth",
                            },
                        },
                        new string[] { }
                    },
        }
    );
});

//var origins = builder.Configuration.GetValue("AllowedOrigins", string.Empty)!.Split(",");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatMessagesApp.Web.FrontOffice v1"));
}
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<MessagingHub>("/messagingHub");

app.Run();
