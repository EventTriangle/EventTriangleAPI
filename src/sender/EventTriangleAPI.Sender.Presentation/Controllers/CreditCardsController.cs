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
public class CreditCardsController : ControllerBase
{
    private readonly AttachCreditCardToAccountCommandHandler _attachCreditCardToAccountCommandHandler;
    private readonly EditCreditCardCommandHandler _editCreditCardCommandHandler;
    private readonly DeleteCreditCardCommandHandler _deleteCreditCardCommandHandler;
    private readonly UserClaimsService _userClaimsService;

    public CreditCardsController(
        UserClaimsService userClaimsService,
        AttachCreditCardToAccountCommandHandler attachCreditCardToAccountCommandHandler,
        DeleteCreditCardCommandHandler deleteCreditCardCommandHandler, 
        EditCreditCardCommandHandler editCreditCardCommandHandler)
    {
        _userClaimsService = userClaimsService;
        _attachCreditCardToAccountCommandHandler = attachCreditCardToAccountCommandHandler;
        _deleteCreditCardCommandHandler = deleteCreditCardCommandHandler;
        _editCreditCardCommandHandler = editCreditCardCommandHandler;
    }

    [HttpPost]
    public async Task<IActionResult> AttachCreditCardToAccount([FromBody] AttachCreditCardToAccountRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new AttachCreditCardToAccountCommand(
            requesterId,
            request.HolderName,
            request.CardNumber,
            request.Expiration,
            request.Cvv,
            request.PaymentNetwork);
        
        var result = await _attachCreditCardToAccountCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
    
    [HttpPut]
    public async Task<IActionResult> EditCreditCard([FromBody] EditCreditCardRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new EditCreditCardCommand(
            requesterId, 
            request.CardId,
            request.HolderName,
            request.CardNumber,
            request.Expiration,
            request.Cvv,
            request.PaymentNetwork);
        
        var result = await _editCreditCardCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }

    [HttpDelete]    
    public async Task<IActionResult> DeleteCreditCard([FromBody] DeleteCreditCardRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new DeleteCreditCardCommand(requesterId, request.CardId);
        var result = await _deleteCreditCardCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
}