using EventTriangleAPI.Authorization.BusinessLogic.Interfaces;
using EventTriangleAPI.Authorization.BusinessLogic.Services;
using EventTriangleAPI.Shared.DTO.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var azureAdConfiguration = builder.Configuration
    .GetSection("AzureAd")
    .Get<AzureAdConfiguration>();

builder.Services.AddScoped<AzureAdConfiguration>(_ => azureAdConfiguration);

builder.Services.AddGrpc();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IAzureAdService, AzureAdService>();

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