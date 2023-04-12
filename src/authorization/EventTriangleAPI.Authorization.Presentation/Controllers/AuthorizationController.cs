using EventTriangleAPI.Authorization.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Authorization.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IAzureAdService _azureAdService;

    public AuthorizationController(IAzureAdService azureAdService)
    {
        _azureAdService = azureAdService;
    }

    [HttpGet("authorization_data")]
    public async Task<IActionResult> GetAuthorizationData([FromQuery] string code, [FromQuery] string codeVerifier)
    {
        var result = await _azureAdService.GetAuthorizationData(code, codeVerifier); 

        return Ok(result);
    }
    
    [HttpGet("refresh_data")]
    public async Task<IActionResult> GetRefreshData([FromQuery] string code, [FromQuery] string codeVerifier)
    {
        var result = await _azureAdService.GetRefreshData(code, codeVerifier); 

        return Ok(result);
    }
}