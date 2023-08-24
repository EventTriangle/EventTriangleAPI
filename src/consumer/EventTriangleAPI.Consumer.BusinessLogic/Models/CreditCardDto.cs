using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public record CreditCardDto(
    Guid Id,
    string UserId,
    string HolderName,
    string CardNumber,
    string Cvv,
    string Expiration,
    PaymentNetwork PaymentNetwork);