using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;
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

var azAdSection = builder.Configuration
    .GetSection("AzureAd");

var azAdConfig = azAdSection.Get<AzureAdConfiguration>();

var keyVaultUrl = builder.Configuration["KeyVaultUrl"];

var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

var clientSecret = await client.GetSecretAsync("AzureAppSecret");

var secretString = clientSecret.Value.Value;

if (string.IsNullOrEmpty(secretString))
{
    throw new ArgumentNullException(nameof(clientSecret));
}

azAdConfig.ClientSecret = secretString;
azAdConfig.AzureAdTokenUrl = $"{azAdConfig.Instance}{azAdConfig.TenantId}/oauth2/v2.0/token";

builder.Services.AddScoped(_ => azAdConfig);

builder.Services.AddScoped<RefreshTokenCommandHandler>();
builder.Services.AddScoped<GetTokenCommandHandler>();

builder.Services.AddGrpc();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventTriangle Authorization API V1"); });


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();