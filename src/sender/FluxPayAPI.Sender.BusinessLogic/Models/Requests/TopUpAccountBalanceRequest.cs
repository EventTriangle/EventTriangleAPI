namespace FluxPayAPI.Sender.BusinessLogic.Models.Requests;

public record TopUpAccountBalanceRequest(Guid CreditCardId, decimal Amount, Guid? OperationId = null);