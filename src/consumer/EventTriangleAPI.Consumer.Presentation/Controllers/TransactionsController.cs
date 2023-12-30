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
public class TransactionsController : ControllerBase
{
    private readonly GetTransactionsQueryHandler _getTransactionsQueryHandler;
    private readonly GetTransactionsBySearchQueryHandler _getTransactionsBySearchQueryHandler;
    private readonly GetTransactionsByUserIdQueryHandler _getTransactionsByUserIdQueryHandler;
    private readonly UserClaimsService _userClaimsService;

    public TransactionsController(
        UserClaimsService userClaimsService, 
        GetTransactionsBySearchQueryHandler getTransactionsBySearchQueryHandler,
        GetTransactionsQueryHandler getTransactionsQueryHandler, 
        GetTransactionsByUserIdQueryHandler getTransactionsByUserIdQueryHandler)
    {
        _userClaimsService = userClaimsService;
        _getTransactionsQueryHandler = getTransactionsQueryHandler;
        _getTransactionsBySearchQueryHandler = getTransactionsBySearchQueryHandler;
        _getTransactionsByUserIdQueryHandler = getTransactionsByUserIdQueryHandler;
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
    
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetTransactionsByUserId(
        string userId,
        [FromQuery] DateTime fromDateTime,
        [FromQuery] int limit = 25)
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetTransactionsByUserIdQuery(requesterId, userId, limit, fromDateTime);
        var result = await _getTransactionsByUserIdQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Returns user's transactions by search.
    /// </summary>
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
    [HttpGet("search/{searchText}")]
    public async Task<IActionResult> GetTransactionsBySearch(
        string searchText,
        [FromQuery] DateTime fromDateTime,
        [FromQuery] int limit = 25)
    {
        var requesterId = _userClaimsService.GetUserId();
        
        var query = new GetTransactionsBySearchQuery(requesterId, searchText, limit, fromDateTime);
        var result = await _getTransactionsBySearchQueryHandler.HandleAsync(query);

        return result.ToActionResult();
    }
}