using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.Models.Requests;

public record EditCreditCardRequest(
    Guid CardId,
    string HolderName,
    string CardNumber,
    string Expiration,
    string Cvv,
    PaymentNetwork PaymentNetwork);