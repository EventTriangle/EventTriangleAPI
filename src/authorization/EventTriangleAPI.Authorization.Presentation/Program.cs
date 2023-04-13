using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EventTriangleAPI.Authorization.BusinessLogic.Interfaces;
using EventTriangleAPI.Authorization.BusinessLogic.Services;
using EventTriangleAPI.Shared.DTO.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddScoped<AzureAdConfiguration>(_ => azAdConfig);

builder.Services.AddGrpc();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IAzureAdService, AzureAdService>();

var jsonSerializerSettings = new JsonSerializerSettings
{
    ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new SnakeCaseNamingStrategy()
    }
};

builder.Services.AddScoped<JsonSerializerSettings>(_ => jsonSerializerSettings);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();