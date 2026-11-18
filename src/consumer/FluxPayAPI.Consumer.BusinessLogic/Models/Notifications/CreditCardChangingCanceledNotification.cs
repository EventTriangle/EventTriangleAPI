using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Consumer.BusinessLogic.Models.Notifications;

public record CreditCardChangingCanceledNotification(
    string HolderName, 
    string CardNumber,
    string Cvv,
    string Expiration,
    PaymentNetwork PaymentNetwork,
    string Reason
);