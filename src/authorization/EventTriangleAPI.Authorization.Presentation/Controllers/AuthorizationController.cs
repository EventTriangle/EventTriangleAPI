using EventTriangleAPI.Authorization.BusinessLogic.Handlers;
using EventTriangleAPI.Authorization.BusinessLogic.Interfaces;
using EventTriangleAPI.Shared.Application.Extensions;
using EventTriangleAPI.Shared.DTO.Commands;
using EventTriangleAPI.Shared.DTO.Commands.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Authorization.Presentation.Controllers;

[Route("api/authorization")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IAzureAdService _azureAdService;
    private readonly RefreshTokenCommandHandler _refreshTokenCommandHandler;
    private readonly GetTokenCommandHandler _getTokenCommandHandler;

    public AuthorizationController(IAzureAdService azureAdService,
        RefreshTokenCommandHandler refreshTokenCommandHandler,
        GetTokenCommandHandler getTokenCommandHandler)
    {
        _azureAdService = azureAdService;
        _refreshTokenCommandHandler = refreshTokenCommandHandler;
        _getTokenCommandHandler = getTokenCommandHandler;
    }

    [HttpGet("token")]
    public async Task<IActionResult> GetAuthorizationData([FromQuery] string code, [FromQuery] string codeVerifier)
    {
        var body = new GetAccessTokenBody(code, codeVerifier);
        var command = new Command<GetAccessTokenBody>(body);
        var result = await _getTokenCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetRefreshData([FromQuery] string refreshToken)
    {
        var body = new RefreshTokenBody(refreshToken);
        var command = new Command<RefreshTokenBody>(body);
        var result = await _refreshTokenCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
}