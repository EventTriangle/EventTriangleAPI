using EventTriangleAPI.Authorization.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Authorization.Presentation.Controllers;

public class RootController : Controller
{
    [HttpGet("/")]
    public IActionResult RedirectToTheAngularSpa()
    {
        return Redirect($@"~{SpaRouting.Transactions}");
    }
}