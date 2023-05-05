using EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using EventTriangleAPI.Shared.DTO.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Authorization.Presentation.Controllers;

[Route("api")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly RefreshTokenCommandHandler _refreshTokenCommandHandler;
    private readonly GetTokenCommandHandler _getTokenCommandHandler;

    public AuthorizationController(
        RefreshTokenCommandHandler refreshTokenCommandHandler,
        GetTokenCommandHandler getTokenCommandHandler)
    {
        _refreshTokenCommandHandler = refreshTokenCommandHandler;
        _getTokenCommandHandler = getTokenCommandHandler;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var authN = await HttpContext.AuthenticateAsync("appOidc");

        if (authN.Succeeded)
        {
            return Redirect("/app/transactions");
        }
        
        return Challenge("appOidc");
    }
    
    [Authorize]
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        return SignOut("appOidc");
    }

    [AllowAnonymous]
    [HttpGet("isAuthenticated")]
    public async Task<IActionResult> IsAuthenticated()
    {
        var authN = await HttpContext.AuthenticateAsync("appOidc");
        return authN.Succeeded 
            ? Ok(new AuthNStatus(true))
            : Unauthorized(new AuthNStatus(false));
    }
    
    [HttpGet("token")]
    public async Task<IActionResult> GetAuthorizationData([FromQuery] string code, [FromQuery] string codeVerifier)
    {
        var body = new GetTokenBody(code, codeVerifier);
        var command = new Command<GetTokenBody>(body);
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
    
    public record AuthNStatus(bool Authenticated);
}