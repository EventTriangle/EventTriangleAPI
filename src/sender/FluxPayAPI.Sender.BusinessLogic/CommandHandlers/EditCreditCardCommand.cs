using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record EditCreditCardCommand(
    string RequesterId,
    Guid CardId,
    string HolderName,
    string CardNumber,
    string Expiration,
    string Cvv,
    PaymentNetwork PaymentNetwork) : ICommand;