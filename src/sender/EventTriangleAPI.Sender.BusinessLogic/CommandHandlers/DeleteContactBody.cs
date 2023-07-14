namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record DeleteContactBody(Guid RequesterId, Guid ContactId);