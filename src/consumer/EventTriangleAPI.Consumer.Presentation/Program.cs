using EventTriangleAPI.Consumer.BusinessLogic.Consumers;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.DTO.Models;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

var configurationSection = builder.Configuration.GetSection(AppSettingsConstants.AzureAd);
var rabbitMqConfiguration = builder.Configuration.GetSection(AppSettingsConstants.RabbitMqConfiguration).Get<RabbitMqConfiguration>();
var databaseConnectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(databaseConnectionString);
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(configurationSection);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<EventConsumer>();
    
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitMqConfiguration.Host, h =>
        {
            h.Username(rabbitMqConfiguration.Username);
            h.Password(rabbitMqConfiguration.Password);
        });
        
        cfg.ReceiveEndpoint("event-queue", c => { c.Consumer<EventConsumer>(); });
    });
});

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();