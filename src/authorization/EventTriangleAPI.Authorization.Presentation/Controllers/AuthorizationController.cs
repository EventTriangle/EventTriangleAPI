using EventTriangleAPI.Authorization.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Authorization.Presentation.Controllers;

[Route("api/authorization")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IAzureAdService _azureAdService;

    public AuthorizationController(IAzureAdService azureAdService)
    {
        _azureAdService = azureAdService;
    }

    [HttpGet("get-token")]
    public async Task<IActionResult> GetAuthorizationData([FromQuery] string code, [FromQuery] string codeVerifier)
    {
        var result = await _azureAdService.GetAccessAndIdTokensAsync(code, codeVerifier); 

        return Ok(result);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> GetRefreshData([FromQuery] string refreshToken)
    {
        var result = await _azureAdService.RefreshAccessAndIdTokensAsync(refreshToken); 

        return Ok(result);
    }
}