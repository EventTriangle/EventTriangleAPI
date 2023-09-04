using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.Models.Requests;

public record AttachCreditCardToAccountRequest(
    string HolderName,
    string CardNumber,
    string Expiration,
    string Cvv,
    PaymentNetwork PaymentNetwork);