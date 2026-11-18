using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Consumer.BusinessLogic.Models;

public record CreditCardDto(
    Guid Id,
    string UserId,
    string HolderName,
    string CardNumber,
    string Cvv,
    string Expiration,
    PaymentNetwork PaymentNetwork);