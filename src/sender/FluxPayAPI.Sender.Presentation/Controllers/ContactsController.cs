using FluxPayAPI.Sender.Application.Services;
using FluxPayAPI.Sender.BusinessLogic.CommandHandlers;
using FluxPayAPI.Sender.BusinessLogic.Models.Requests;
using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Shared.Application.Extensions;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluxPayAPI.Sender.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly AddContactCommandHandler _addContactCommandHandler;
    private readonly DeleteContactCommandHandler _deleteContactCommandHandler;
    private readonly UserClaimsService _userClaimsService;

    public ContactsController(
        UserClaimsService userClaimsService, 
        AddContactCommandHandler addContactCommandHandler, 
        DeleteContactCommandHandler deleteContactCommandHandler)
    {
        _userClaimsService = userClaimsService;
        _addContactCommandHandler = addContactCommandHandler;
        _deleteContactCommandHandler = deleteContactCommandHandler;
    }

    /// <summary>
    /// Add contact.
    /// </summary>
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ContactCreatedEvent), StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> AddContact([FromBody] AddContactRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new AddContactCommand(requesterId, request.ContactId);
        var result = await _addContactCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Delete contact.
    /// </summary>
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ContactDeletedEvent), StatusCodes.Status200OK)]
    [HttpDelete]
    public async Task<IActionResult> DeleteContact([FromBody] DeleteContactRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new DeleteContactCommand(requesterId, request.ContactId);
        var result = await _deleteContactCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
}