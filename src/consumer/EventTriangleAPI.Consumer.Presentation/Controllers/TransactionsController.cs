using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Consumer.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly GetTransactionsQueryHandler _getTransactionsQueryHandler;
    private readonly UserClaimsService _userClaimsService;

    public TransactionsController(
        UserClaimsService userClaimsService, 
        GetTransactionsQueryHandler getTransactionsQueryHandler)
    {
        _userClaimsService = userClaimsService;
        _getTransactionsQueryHandler = getTransactionsQueryHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] DateTime fromDateTime,
        [FromQuery] int limit = 25)
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetTransactionsQuery(requesterId, limit, fromDateTime);
        var result = await _getTransactionsQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}