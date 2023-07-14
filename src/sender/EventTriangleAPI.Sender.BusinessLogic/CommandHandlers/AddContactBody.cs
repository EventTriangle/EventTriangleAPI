namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record AddContactBody(Guid RequesterId, Guid ContactId);