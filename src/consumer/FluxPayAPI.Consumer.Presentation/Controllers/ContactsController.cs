using FluxPayAPI.Consumer.Application.Services;
using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Shared.Application.Extensions;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluxPayAPI.Consumer.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly GetContactsQueryHandler _getContactsQueryHandler;
    private readonly GetContactsBySearchQueryHandler _getContactsBySearchQueryHandler;
    private readonly UserClaimsService _userClaimsService;
    
    public ContactsController(
        UserClaimsService userClaimsService, 
        GetContactsQueryHandler getContactsQueryHandler,
        GetContactsBySearchQueryHandler getContactsBySearchQueryHandler)
    {
        _userClaimsService = userClaimsService;
        _getContactsQueryHandler = getContactsQueryHandler;
        _getContactsBySearchQueryHandler = getContactsBySearchQueryHandler;
    }
    
    /// <summary>
    /// Returns all user's contacts.
    /// </summary>
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ContactDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetContactsQuery(requesterId);
        var result = await _getContactsQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Returns all contacts by email.
    /// </summary>
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ContactDto>), StatusCodes.Status200OK)]
    [HttpGet("search")]
    public async Task<IActionResult> GetContactsBySearch([FromQuery] string email)
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetContactsBySearchQuery(requesterId, email);
        var result = await _getContactsBySearchQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}