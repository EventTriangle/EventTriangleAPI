using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record OpenSupportTicketCommand(
    string RequesterId, 
    Guid WalletId,
    string TicketReason) : ICommand;