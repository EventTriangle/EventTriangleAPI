using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Consumer.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private readonly GetProfileQueryHandler _getProfileQueryHandler;
    private readonly GetProfileByIdQueryHandler _getProfileByIdQueryHandler;
    private readonly UserClaimsService _userClaimsService;
    
    public ProfileController(
        UserClaimsService userClaimsService, 
        GetProfileQueryHandler getProfileQueryHandler, 
        GetProfileByIdQueryHandler getProfileByIdQueryHandler)
    {
        _userClaimsService = userClaimsService;
        _getProfileQueryHandler = getProfileQueryHandler;
        _getProfileByIdQueryHandler = getProfileByIdQueryHandler;
    }

    /// <summary>
    /// Returns user's profile.
    /// </summary>
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetProfileQuery(requesterId);
        var result = await _getProfileQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Returns user's profile by Id.
    /// </summary>
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetProfileById(string userId)
    {
        var query = new GetProfileByIdQuery(userId);
        var result = await _getProfileByIdQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}