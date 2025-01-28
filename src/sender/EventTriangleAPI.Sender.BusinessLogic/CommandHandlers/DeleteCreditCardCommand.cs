using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record DeleteCreditCardCommand(string RequesterId, Guid CardId) : ICommand;