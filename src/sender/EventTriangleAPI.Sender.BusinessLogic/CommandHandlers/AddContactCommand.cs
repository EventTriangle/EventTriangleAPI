using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record AddContactCommand(string RequesterId, string ContactId) : ICommand;