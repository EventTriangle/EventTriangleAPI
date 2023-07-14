namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record DeleteCreditCardBody(Guid RequesterId, Guid CardId);