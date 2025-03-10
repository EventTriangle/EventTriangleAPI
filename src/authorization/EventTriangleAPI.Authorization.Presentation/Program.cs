using EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Authorization.Domain.Constants;
using EventTriangleAPI.Authorization.Persistence;
using EventTriangleAPI.Authorization.Presentation.DependencyInjection;
using EventTriangleAPI.Shared.DTO.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var azAdSection = builder.Configuration.GetSection(AppSettingsConstants.AzureAdSelection);
var azAdConfig = azAdSection.Get<AzureAdConfiguration>();
var adClientSecret = Environment.GetEnvironmentVariable(AppSettingsConstants.AdSecretKey);
var reverseProxySection = builder.Configuration.GetSection(AppSettingsConstants.ReverseProxySelection);
var databaseConnectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

var allowOrigins = builder.Configuration.GetSection(AppSettingsConstants.AllowedOrigins).Get<string[]>();

azAdConfig.ClientSecret = adClientSecret;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddSpaStaticFiles(config => { config.RootPath = "wwwroot"; });
builder.Services.AddMvc();
builder.Services.ConfigureYarp(reverseProxySection);
builder.Services.ConfigureCors(allowOrigins);
builder.Services.ConfigureSameSiteNoneCookiePolicy();
builder.Services.AddDbContext<DatabaseContext>(options => { options.UseNpgsql(databaseConnectionString); });

if (string.IsNullOrEmpty(adClientSecret))
{
    throw new ArgumentNullException(nameof(adClientSecret));
}

builder.Services.AddAppAuthentication(
    azAdConfig.Authority,
    azAdConfig.ClientId.ToString(),
    azAdConfig.ClientSecret,
    azAdConfig.CallbackPath,
    azAdConfig.Scopes,
    builder.Environment.IsDevelopment());

builder.Services.AddSingleton<IMemoryCache>(_ => new MemoryCache(new MemoryCacheOptions()));

builder.Services.AddScoped(_ => azAdConfig);
builder.Services.AddScoped<RefreshTokenCommandHandler>();
builder.Services.AddScoped<GetTokenCommandHandler>();

builder.Services.AddGrpc();
builder.Services.AddHttpClient();

builder.Services.AddTicketStore();
builder.Services.AddHostedServices();

builder.Logging.AddFilter("Grpc", LogLevel.Debug);

// redis configs start

var redisUrl = builder.Configuration[AppSettingsConstants.RedisUrl];

if (string.IsNullOrEmpty(redisUrl))
{
    throw new ArgumentNullException(nameof(redisUrl));
}

var redisPassword = builder.Configuration[AppSettingsConstants.RedisPassword];

if (string.IsNullOrEmpty(redisPassword))
{
    throw new ArgumentNullException(nameof(redisPassword));
}

var redis = ConnectionMultiplexer.Connect(new ConfigurationOptions
{
    EndPoints = { redisUrl },
    Password = redisPassword,
    AbortOnConnectFail = false
});

builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

// redis configs end

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventTriangle Authorization API V1"); });

app.UseHttpsRedirection();

app.UseHsts();

app.UseStaticFiles();

app.UseSpaStaticFiles();

app.UseCookiePolicy();

app.UseRouting();

app.UseCors(CorsServices.CorsPolicyName);

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();
app.MapControllers();

app.MigrateDatabase();

app.Map(SpaRouting.Transactions, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Cards, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Deposit, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Contacts, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Support, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Tickets, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Users, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));

app.Run();
