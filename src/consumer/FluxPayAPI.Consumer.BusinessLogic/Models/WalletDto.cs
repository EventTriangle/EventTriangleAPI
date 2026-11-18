namespace FluxPayAPI.Consumer.BusinessLogic.Models;

public record WalletDto(
    Guid Id,
    decimal Balance,
    Guid? LastTransactionId);
