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