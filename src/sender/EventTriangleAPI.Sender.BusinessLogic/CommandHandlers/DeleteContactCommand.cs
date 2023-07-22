using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record DeleteContactCommand(string UserId, string ContactId) : ICommand;