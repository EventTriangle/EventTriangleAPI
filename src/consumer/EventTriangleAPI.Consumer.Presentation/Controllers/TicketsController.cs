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
public class TicketsController : ControllerBase
{
    private readonly GetSupportTicketsQueryHandler _getSupportTicketsQueryHandler;
    private readonly GetTicketsQueryHandler _getTicketsQueryHandler;
    private readonly UserClaimsService _userClaimsService;

    public TicketsController(
        UserClaimsService userClaimsService, 
        GetTicketsQueryHandler getTicketsQueryHandler, 
        GetSupportTicketsQueryHandler getSupportTicketsQueryHandler)
    {
        _userClaimsService = userClaimsService;
        _getTicketsQueryHandler = getTicketsQueryHandler;
        _getSupportTicketsQueryHandler = getSupportTicketsQueryHandler;
    }

    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<SupportTicketDto>), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
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
    
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<SupportTicketDto>), StatusCodes.Status200OK)]
    [HttpGet("support-tickets")]
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