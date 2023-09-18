using System.Reflection;
using EventTriangleAPI.Sender.Application.Services;
using EventTriangleAPI.Sender.BusinessLogic.GrpcServices;
using EventTriangleAPI.Sender.Domain.Constants;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Sender.Presentation.DependencyInjection;
using EventTriangleAPI.Sender.Presentation.Routing;
using EventTriangleAPI.Shared.DTO.Models;
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
var rabbitMqConfiguration = builder.Configuration.GetSection(AppSettingsConstants.RabbitMqConfiguration).Get<RabbitMqConfiguration>();

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

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitMqConfiguration.Host, h =>
        {
            h.Username(rabbitMqConfiguration.Username);
            h.Password(rabbitMqConfiguration.Password);
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
app.MapGrpcService<UserGrpcService>();

app.MigrateDatabase();

app.Run();