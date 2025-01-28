using EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Authorization.Domain.Constants;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventTriangleAPI.Authorization.Presentation.Controllers;

[Route("api")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly RefreshTokenCommandHandler _refreshTokenCommandHandler;
    private readonly GetTokenCommandHandler _getTokenCommandHandler;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IConfiguration _configuration;

    public AuthorizationController(
        RefreshTokenCommandHandler refreshTokenCommandHandler,
        GetTokenCommandHandler getTokenCommandHandler,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration)
    {
        _refreshTokenCommandHandler = refreshTokenCommandHandler;
        _getTokenCommandHandler = getTokenCommandHandler;
        _hostEnvironment = hostEnvironment;
        _configuration = configuration;
    }

    [HttpGet("login")]
    [SwaggerOperation(Summary = "Logs user in via Azure AD.")]
    public async Task<IActionResult> Login()
    {
        var authN = await HttpContext.AuthenticateAsync(AuthConstants.AppOidc);
        var devFrontendUrl = _configuration[AppSettingsConstants.DevFrontendUrl];

        if (authN.Succeeded)
        {
            return _hostEnvironment.IsDevelopment()
                ? Redirect(devFrontendUrl + SpaRouting.Transactions)
                : Redirect(SpaRouting.Transactions);
        }

        return Challenge(AuthConstants.AppOidc);
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        var devFrontendUrl = _configuration[AppSettingsConstants.DevFrontendUrl];

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return _hostEnvironment.IsDevelopment()
            ? SignOut(new AuthenticationProperties { RedirectUri = devFrontendUrl + SpaRouting.Transactions },
                AuthConstants.AppOidc)
            : SignOut(AuthConstants.AppOidc);
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
        var command = new GetTokenCommand(code, codeVerifier);
        var result = await _getTokenCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetRefreshData([FromQuery] string refreshToken)
    {
        var command = new RefreshTokenCommand(refreshToken);
        var result = await _refreshTokenCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }

    private record AuthNStatus(bool Authenticated);
}