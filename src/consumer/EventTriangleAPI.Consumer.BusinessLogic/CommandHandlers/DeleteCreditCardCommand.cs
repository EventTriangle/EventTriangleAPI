using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record DeleteCreditCardCommand(string UserId, Guid CardId) : ICommand;