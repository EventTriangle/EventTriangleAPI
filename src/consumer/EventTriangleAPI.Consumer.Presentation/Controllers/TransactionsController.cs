using EventTriangleAPI.Consumer.Application.Services;
using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.Application.Extensions;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

    /// <summary>
    /// Returns user's transactions.
    /// </summary>
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
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