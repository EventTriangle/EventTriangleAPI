using EventTriangleAPI.Sender.Domain.Constants;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Sender.Presentation.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

var configurationSection = builder.Configuration.GetSection("AzureAd");
var databaseConnectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(databaseConnectionString);
});
builder.Services.AddCommandHandlers();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(configurationSection);

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();