using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record OpenSupportTicketCommand(
    string UserId, 
    Guid WalletId,
    string TicketReason) : ICommand;