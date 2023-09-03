using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Consumer.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private readonly GetProfileQueryHandler _getProfileQueryHandler;
    private readonly UserClaimsService _userClaimsService;
    
    public ProfileController(
        UserClaimsService userClaimsService, 
        GetProfileQueryHandler getProfileQueryHandler)
    {
        _userClaimsService = userClaimsService;
        _getProfileQueryHandler = getProfileQueryHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetProfileQuery(requesterId);
        var result = await _getProfileQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}