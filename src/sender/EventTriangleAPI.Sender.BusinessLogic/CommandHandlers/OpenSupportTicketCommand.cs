using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record OpenSupportTicketCommand(
    string UserId, 
    string Username,
    Guid WalletId,
    string TicketReason) : ICommand;