using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record EditCreditCardCommand(
    string RequesterId,
    Guid CardId,
    string HolderName,
    string CardNumber,
    string Expiration,
    string Cvv,
    PaymentNetwork PaymentNetwork) : ICommand;