using EventTriangleAPI.Shared.DTO.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var azAdSection = builder.Configuration.GetSection("AzureAd");
var azAdConfig = azAdSection.Get<AzureAdConfiguration>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Audience = azAdConfig.ClientId.ToString();
        options.Authority = $"https://sts.windows.net/{azAdConfig.TenantId}/";
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = azAdConfig.ClientId.ToString(),
            ValidateIssuer = true,
            ValidIssuer = $"https://sts.windows.net/{azAdConfig.TenantId}/",
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false
        };
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