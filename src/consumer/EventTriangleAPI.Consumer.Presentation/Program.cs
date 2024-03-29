using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.Consumers;
using EventTriangleAPI.Consumer.BusinessLogic.Hubs;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Consumer.Presentation.DependencyInjection;
using EventTriangleAPI.Consumer.Presentation.Routing;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

var configurationSection = builder.Configuration.GetSection(AppSettingsConstants.AzureAd);
var databaseConnectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];
var shouldCreateSeeds = builder.Configuration.GetValue<bool>(AppSettingsConstants.ShouldCreateSeeds);

builder.Services.AddControllers(o =>
{
    o.Conventions.Add(new RouteTokenTransformerConvention(new CustomParameterTransformer()));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddSignalR();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(databaseConnectionString);
});

builder.Services.AddCommandHandlers();
builder.Services.AddQueryHandlers();
builder.Services.AddTransient<UserClaimsService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("SignalR", options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.Request.Path;
    
                if (path.StartsWithSegments("/notify") && !string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
    
                return Task.CompletedTask;
            }
        };
    })
    .AddMicrosoftIdentityWebApi(configurationSection);

var rabbitHost = builder.Configuration[AppSettingsConstants.RabbitMqHost];
var rabbitUsername = builder.Configuration[AppSettingsConstants.RabbitMqUsername];
var rabbitPassword = builder.Configuration[AppSettingsConstants.RabbitMqPassword];

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<EventConsumer>();
    
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitHost, h =>
        {
            h.Username(rabbitUsername);
            h.Password(rabbitPassword);
        });
        
        cfg.ReceiveEndpoint("event-queue", c =>
        {
            c.PrefetchCount = 1;
            c.UseConcurrencyLimit(1);
            c.ConfigureConsumer<EventConsumer>(ctx);
        });
    });
});

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("notify");

app.MigrateDatabase();

if (shouldCreateSeeds)
{
    await app.InitializeSeeds();
}

app.Run();