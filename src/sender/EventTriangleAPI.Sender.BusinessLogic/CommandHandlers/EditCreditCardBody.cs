using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record EditCreditCardBody(
    Guid RequesterId,
    Guid CardId,
    string HolderName,
    string CardNumber,
    string Expiration,
    string Cvv,
    PaymentNetwork PaymentNetwork);