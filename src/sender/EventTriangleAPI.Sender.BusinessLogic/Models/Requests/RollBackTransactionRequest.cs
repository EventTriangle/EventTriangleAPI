namespace EventTriangleAPI.Sender.BusinessLogic.Models.Requests;

public record RollBackTransactionRequest(Guid TransactionId);