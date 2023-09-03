using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Consumer.Presentation.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("[controller]")]
public class TicketsController : ControllerBase
{
    private readonly GetTicketsQueryHandler _getTicketsQueryHandler;
    private readonly UserClaimsService _userClaimsService;

    public TicketsController(
        UserClaimsService userClaimsService, 
        GetTicketsQueryHandler getTicketsQueryHandler)
    {
        _userClaimsService = userClaimsService;
        _getTicketsQueryHandler = getTicketsQueryHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetTickets(
        [FromQuery] DateTime fromDateTime,
        [FromQuery] int limit = 25)
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetTicketsQuery(requesterId, limit, fromDateTime);
        var result = await _getTicketsQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}