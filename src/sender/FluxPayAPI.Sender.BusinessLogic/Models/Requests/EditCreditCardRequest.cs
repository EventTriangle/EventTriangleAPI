using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Sender.BusinessLogic.Models.Requests;

public record EditCreditCardRequest(
    Guid CardId,
    string HolderName,
    string CardNumber,
    string Expiration,
    string Cvv,
    PaymentNetwork PaymentNetwork);