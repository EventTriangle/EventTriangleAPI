using EventTriangleAPI.Authorization.BusinessLogic.Protos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace EventTriangleAPI.Sender.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("receive_token")]
    public async Task<IActionResult> ReceiveToken([FromQuery] string code)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7000");
        var client = new AzureAd.AzureAdClient(channel);

        var azureAdRequest = new AzureAdRequest {Code = code, CodeVerifier = "123"};
        
        var result = await client.ReceiveTokenAsync(azureAdRequest);
        
        return Ok(result.Token);
    } 

    [Authorize(Roles = "User, Admin")]
    [HttpGet("user_and_admin")]
    public IEnumerable<WeatherForecast> GetForUserAndAdmin()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IEnumerable<WeatherForecast> GetForAdmin()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}