using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record AttachCreditCardToAccountCommand(
    string RequesterId,
    string HolderName,
    string CardNumber,
    string Expiration,
    string Cvv,
    PaymentNetwork PaymentNetwork) : ICommand;