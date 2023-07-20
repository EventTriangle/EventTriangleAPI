using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record DeleteCreditCardCommand(string UserId, Guid CardId) : ICommand;