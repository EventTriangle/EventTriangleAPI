using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record AddCreditCardCommand(
    string RequesterId,
    string HolderName, 
    string CardNumber,
    string Cvv,
    string Expiration,
    PaymentNetwork PaymentNetwork) : ICommand;