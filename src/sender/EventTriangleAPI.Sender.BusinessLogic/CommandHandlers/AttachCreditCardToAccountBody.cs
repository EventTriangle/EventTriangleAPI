using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record AttachCreditCardToAccountBody(
    string UserSub,
    string HolderName,
    string CardNumber,
    string Expiration,
    string Cvv,
    PaymentNetwork PaymentNetwork);