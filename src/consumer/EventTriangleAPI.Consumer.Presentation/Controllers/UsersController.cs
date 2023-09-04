using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Consumer.Presentation.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly GetUsersQueryHandler _getUsersQueryHandler;
    private readonly GetUsersBySearchQueryHandler _getUsersBySearchQueryHandler;
    private readonly UserClaimsService _userClaimsService;

    public UsersController(
        UserClaimsService userClaimsService,
        GetUsersQueryHandler getUsersQueryHandler,
        GetUsersBySearchQueryHandler getUsersBySearchQueryHandler)
    {
        _userClaimsService = userClaimsService;
        _getUsersQueryHandler = getUsersQueryHandler;
        _getUsersBySearchQueryHandler = getUsersBySearchQueryHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int limit = 25,
        [FromQuery] int page = 1)
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetUsersQuery(requesterId, limit, page);
        var result = await _getUsersQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
    
    [HttpGet("search/{searchText}")]
    public async Task<IActionResult> GetUsers(
        string searchText,
        [FromQuery] int limit = 25,
        [FromQuery] int page = 1)
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetUsersBySearchQuery(requesterId, searchText, limit, page);
        var result = await _getUsersBySearchQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}