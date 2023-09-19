using System.Reflection;
using EventTriangleAPI.Sender.Application.Services;
using EventTriangleAPI.Sender.BusinessLogic.GrpcServices;
using EventTriangleAPI.Sender.Domain.Constants;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Sender.Presentation.DependencyInjection;
using EventTriangleAPI.Sender.Presentation.Routing;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.EnvironmentName == "Docker")
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(80, listenOptions => listenOptions.Protocols = HttpProtocols.Http1);
        options.ListenAnyIP(81, listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
    });
}

var configurationSection = builder.Configuration.GetSection(AppSettingsConstants.AzureAd);
var databaseConnectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

builder.Services.AddControllers(o =>
{
    o.Conventions.Add(new RouteTokenTransformerConvention(new CustomParameterTransformer()));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(databaseConnectionString);
});
builder.Services.AddCommandHandlers();
builder.Services.AddGrpc();
builder.Services.AddTransient<UserClaimsService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(configurationSection);

var rabbitHost = builder.Configuration[AppSettingsConstants.RabbitMqHost];
var rabbitUsername = builder.Configuration[AppSettingsConstants.RabbitMqUsername];
var rabbitPassword = builder.Configuration[AppSettingsConstants.RabbitMqPassword];

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitHost, h =>
        {
            h.Username(rabbitUsername);
            h.Password(rabbitPassword);
        });
    });
});

builder.WebHost.ConfigureLogging(logging =>
{
    logging.AddFilter("Grpc", LogLevel.Debug);
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
app.MapGrpcService<UserGrpcService>();

app.MigrateDatabase();

app.Run();