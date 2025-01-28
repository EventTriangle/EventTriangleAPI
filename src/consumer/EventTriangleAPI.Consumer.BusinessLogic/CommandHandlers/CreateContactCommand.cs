using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record CreateContactCommand(string RequesterId, string ContactId) : ICommand;