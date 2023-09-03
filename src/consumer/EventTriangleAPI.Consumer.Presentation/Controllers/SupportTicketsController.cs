using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Consumer.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SupportTicketsController : ControllerBase
{
    private readonly GetSupportTicketsQueryHandler _getSupportTicketsQueryHandler;
    private readonly UserClaimsService _userClaimsService;

    public SupportTicketsController(
        UserClaimsService userClaimsService,
        GetSupportTicketsQueryHandler getSupportTicketsQueryHandler)
    {
        _userClaimsService = userClaimsService;
        _getSupportTicketsQueryHandler = getSupportTicketsQueryHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetSupportTickets(
        [FromQuery] DateTime fromDateTime,
        [FromQuery] int limit = 25)
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetSupportsTicketsQuery(requesterId, limit, fromDateTime);
        var result = await _getSupportTicketsQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}