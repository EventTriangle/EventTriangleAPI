namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record DeleteCreditCardBody(string UserId, Guid CardId);