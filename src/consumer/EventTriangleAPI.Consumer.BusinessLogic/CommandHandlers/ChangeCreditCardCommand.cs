using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record ChangeCreditCardCommand(
    Guid CardId, 
    string RequesterId,
    string HolderName, 
    string CardNumber,
    string Cvv,
    string Expiration,
    PaymentNetwork PaymentNetwork) : ICommand;