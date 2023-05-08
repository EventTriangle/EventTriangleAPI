using EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Authorization.Presentation.Constants;
using EventTriangleAPI.Shared.Application.Extensions;
using EventTriangleAPI.Shared.DTO.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        var authN = await HttpContext.AuthenticateAsync(AuthConstants.AppOidc);

        if (authN.Succeeded)
        {
            return Redirect(SpaRouting.Transactions);
        }
        
        return Challenge(AuthConstants.AppOidc);
    }
    
    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return SignOut(AuthConstants.AppOidc);
    }

    [AllowAnonymous]
    [HttpGet("isAuthenticated")]
    public async Task<IActionResult> IsAuthenticated()
    {
        var authN = await HttpContext.AuthenticateAsync(AuthConstants.AppOidc);
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
    
    private record AuthNStatus(bool Authenticated);
}