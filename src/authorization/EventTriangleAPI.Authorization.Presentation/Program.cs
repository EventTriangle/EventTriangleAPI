using EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Authorization.Domain.Constants;
using EventTriangleAPI.Authorization.Persistence;
using EventTriangleAPI.Authorization.Presentation.DependencyInjection;
using EventTriangleAPI.Shared.DTO.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var azAdSection = builder.Configuration.GetSection(AppSettingsConstants.AzureAdSelection);
var azAdConfig = azAdSection.Get<AzureAdConfiguration>();
var adClientSecret = Environment.GetEnvironmentVariable(AppSettingsConstants.AdSecretKey);
var allowedHosts = builder.Configuration[AppSettingsConstants.AllowedHosts];
var reverseProxySection = builder.Configuration.GetSection(AppSettingsConstants.ReverseProxySelection);
var databaseConnectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

azAdConfig.ClientSecret = adClientSecret;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddSpaStaticFiles(config => { config.RootPath = "wwwroot"; });
builder.Services.AddMvc();
builder.Services.AddMemoryCache();
builder.Services.ConfigureYarp(reverseProxySection);
builder.Services.ConfigureCors(allowedHosts);
builder.Services.ConfigureSameSiteNoneCookiePolicy();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(databaseConnectionString);
});

if (string.IsNullOrEmpty(adClientSecret))
{
    throw new ArgumentNullException(nameof(adClientSecret));
}

builder.Services.AddAppAuthentication(
    azAdConfig.Authority,
    azAdConfig.ClientId.ToString(),
    azAdConfig.ClientSecret,
    azAdConfig.CallbackPath,
    azAdConfig.Scopes);

builder.Services.AddScoped(_ => azAdConfig);
builder.Services.AddScoped<RefreshTokenCommandHandler>();
builder.Services.AddScoped<GetTokenCommandHandler>();

builder.Services.AddGrpc();
builder.Services.AddHttpClient();

builder.Services.AddTicketStore();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventTriangle Authorization API V1"); });

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSpaStaticFiles();

app.UseCookiePolicy();

app.UseRouting();

app.UseCors(CorsServices.CorsPolicyName);

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(options =>
{
    options.MapReverseProxy();
    options.MapControllers();    
});

app.MigrateDatabase();

app.Map(SpaRouting.Transactions, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Cards, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Deposit, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Contacts, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Support, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Tickets, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Users, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));

app.Run();