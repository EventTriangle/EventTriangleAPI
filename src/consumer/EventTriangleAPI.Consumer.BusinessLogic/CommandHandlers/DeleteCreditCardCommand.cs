using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record DeleteCreditCardCommand(string RequesterId, Guid CardId) : ICommand;