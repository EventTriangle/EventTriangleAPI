using EventTriangleAPI.Sender.Application.Services;
using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Sender.BusinessLogic.Models.Requests;
using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Shared.Application.Extensions;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Sender.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TicketsController : ControllerBase
{
    private readonly OpenSupportTicketCommandHandler _openSupportTicketCommandHandler;
    private readonly ResolveSupportTicketCommandHandler _resolveSupportTicketCommandHandler;
    private readonly UserClaimsService _userClaimsService;

    public TicketsController(
        UserClaimsService userClaimsService, 
        ResolveSupportTicketCommandHandler resolveSupportTicketCommandHandler, 
        OpenSupportTicketCommandHandler openSupportTicketCommandHandler)
    {
        _userClaimsService = userClaimsService;
        _resolveSupportTicketCommandHandler = resolveSupportTicketCommandHandler;
        _openSupportTicketCommandHandler = openSupportTicketCommandHandler;
    }

    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SupportTicketOpenedEvent), StatusCodes.Status200OK)]
    [HttpPost("support-ticket")]
    public async Task<IActionResult> OpenSupportTicket([FromBody] OpenSupportTicketRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new OpenSupportTicketCommand(requesterId, request.WalletId, request.TransactionId, request.TicketReason);
        var result = await _openSupportTicketCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
    
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SupportTicketResolvedEvent), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    [HttpPut("support-ticket")]
    public async Task<IActionResult> ResolveSupportTicket([FromBody] ResolveSupportTicketRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new ResolveSupportTicketCommand(requesterId, request.TicketId, request.TicketJustification);
        var result = await _resolveSupportTicketCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
}