using ChatMessagesApp.Core.Application;
using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Common;
using ChatMessagesApp.Infrastructure;
using ChatMessagesApp.Web.FrontOffice.Hubs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
    hubOptions.MaximumParallelInvocationsPerClient = 2;
    hubOptions.StreamBufferCapacity = 10;
}).AddJsonProtocol(opts =>
{
    opts.PayloadSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddCommon();
builder.Services.AddApplication();

builder.Services.AddScoped<ISignalRService, SignalRService>();

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

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/Hub")
    {

        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("Content-Security-Policy",
            "default-src 'self'");
    }
    await next();
});

app.MapControllers();
app.MapHub<SignalRHub>("/Hub");
//app.MapHub<MessagingHub>("/messagingHub");

app.Run();
