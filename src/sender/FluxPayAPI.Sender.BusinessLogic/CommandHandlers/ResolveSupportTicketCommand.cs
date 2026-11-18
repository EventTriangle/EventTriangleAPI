using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record ResolveSupportTicketCommand(string RequesterId, Guid TicketId, string TicketJustification) : ICommand;