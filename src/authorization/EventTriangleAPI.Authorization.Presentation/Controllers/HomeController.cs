using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Authorization.Presentation.Controllers;

[Route("api")]
public class HomeController : Controller
{
    [HttpGet("login")]
    public IActionResult Login()
    {
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
    public async Task<AuthNStatus> IsAuthenticated()
    {
        var authN = await HttpContext.AuthenticateAsync("appOidc");
        return authN.Succeeded 
            ? new AuthNStatus(true)
            : new AuthNStatus(false);
    }

    public record AuthNStatus(bool Authenticated);
}