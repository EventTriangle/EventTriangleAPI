using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record DeleteContactCommand(
    string RequesterId, 
    string ContactId) : ICommand;