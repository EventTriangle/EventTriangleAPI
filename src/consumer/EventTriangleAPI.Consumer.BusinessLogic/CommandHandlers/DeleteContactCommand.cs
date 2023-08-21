using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record DeleteContactCommand(string RequesterId, string ContactId) : ICommand;