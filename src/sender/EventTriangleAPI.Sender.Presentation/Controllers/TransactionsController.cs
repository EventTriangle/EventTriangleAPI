using EventTriangleAPI.Sender.Application.Services;
using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Sender.BusinessLogic.Models.Requests;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Sender.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly RollBackTransactionCommandHandler _rollBackTransactionCommandHandler;
    private readonly CreateTransactionUserToUserCommandHandler _createTransactionUserToUserCommandHandler;
    private readonly TopUpAccountBalanceCommandHandler _topUpAccountBalanceCommandHandler;
    private readonly UserClaimsService _userClaimsService;

    public TransactionsController(
        UserClaimsService userClaimsService, 
        CreateTransactionUserToUserCommandHandler createTransactionUserToUserCommandHandler,
        TopUpAccountBalanceCommandHandler topUpAccountBalanceCommandHandler, 
        RollBackTransactionCommandHandler rollBackTransactionCommandHandler)
    {
        _userClaimsService = userClaimsService;
        _createTransactionUserToUserCommandHandler = createTransactionUserToUserCommandHandler;
        _topUpAccountBalanceCommandHandler = topUpAccountBalanceCommandHandler;
        _rollBackTransactionCommandHandler = rollBackTransactionCommandHandler;
    }

    [HttpPost("user-to-user")]
    public async Task<IActionResult> CreateTransactionUserToUser([FromBody] CreateTransactionUserToUserRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new CreateTransactionUserToUserCommand(requesterId, request.ToUserId, request.Amount);
        var result = await _createTransactionUserToUserCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
    
    [HttpPost("card-to-user")]
    public async Task<IActionResult> TopUpAccountBalance([FromBody] TopUpAccountBalanceRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new TopUpAccountBalanceCommand(requesterId, request.CreditCardId, request.Amount);
        var result = await _topUpAccountBalanceCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("rollback")]
    public async Task<IActionResult> RollBackTransaction([FromBody] RollBackTransactionRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new RollBackTransactionCommand(requesterId, request.TransactionId);
        var result = await _rollBackTransactionCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
}
