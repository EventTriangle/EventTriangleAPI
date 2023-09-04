using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Consumer.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly GetContactsQueryHandler _getContactsQueryHandler;
    private readonly UserClaimsService _userClaimsService;
    
    public ContactsController(
        GetContactsQueryHandler getContactsQueryHandler,
        UserClaimsService userClaimsService)
    {
        _getContactsQueryHandler = getContactsQueryHandler;
        _userClaimsService = userClaimsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetContactsQuery(requesterId);
        var result = await _getContactsQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}