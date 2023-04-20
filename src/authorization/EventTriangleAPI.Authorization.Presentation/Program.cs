using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Authorization.Presentation.Constants;
using EventTriangleAPI.Shared.DTO.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "EventTriangle Authorization API", Version = "v1" });
});
builder.Services.AddSpaStaticFiles(config => { config.RootPath = "wwwroot"; });
builder.Services.AddMvc();

var azAdSection = builder.Configuration
    .GetSection("AzureAd");

var azAdConfig = azAdSection.Get<AzureAdConfiguration>();

var adClientSecret = Environment.GetEnvironmentVariable("EVENT_TRIANGLE_AD_CLIENT_SECRET");

if (string.IsNullOrEmpty(adClientSecret))
{
    throw new ArgumentNullException(nameof(adClientSecret));
}

azAdConfig.ClientSecret = adClientSecret;
azAdConfig.AzureAdTokenUrl = $"{azAdConfig.Instance}/{azAdConfig.TenantId}/oauth2/v2.0/token";

builder.Services.AddScoped(_ => azAdConfig);

builder.Services.AddScoped<RefreshTokenCommandHandler>();
builder.Services.AddScoped<GetTokenCommandHandler>();

builder.Services.AddGrpc();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventTriangle Authorization API V1"); });

app.UseStaticFiles();

app.UseSpaStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Map(SpaRouting.Transactions, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Cards, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Deposit, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Contacts, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Support, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Tickets, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Users, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));

app.Run();