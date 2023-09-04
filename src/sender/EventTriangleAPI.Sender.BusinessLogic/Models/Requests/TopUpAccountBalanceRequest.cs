namespace EventTriangleAPI.Sender.BusinessLogic.Models.Requests;

public record TopUpAccountBalanceRequest(Guid CreditCardId, decimal Amount);