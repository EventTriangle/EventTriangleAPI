namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public record WalletDto(
    Guid Id,
    decimal Balance,
    Guid? LastTransactionId);
