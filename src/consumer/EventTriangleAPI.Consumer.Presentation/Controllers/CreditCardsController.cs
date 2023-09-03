using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Consumer.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CreditCardsController : ControllerBase
{
    private readonly GetCreditCardsQueryHandler _getCreditCardsQueryHandler;
    private readonly UserClaimsService _userClaimsService;
    
    public CreditCardsController(
        GetCreditCardsQueryHandler getCreditCardsQueryHandler,
        UserClaimsService userClaimsService)
    {
        _getCreditCardsQueryHandler = getCreditCardsQueryHandler;
        _userClaimsService = userClaimsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCreditCards()
    {
        var requesterId = _userClaimsService.GetUserId();

        var query = new GetCreditCardsQuery(requesterId);
        var result = await _getCreditCardsQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}