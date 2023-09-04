namespace EventTriangleAPI.Sender.BusinessLogic.Models.Requests;

public record CreateTransactionUserToUserRequest(string ToUserId, decimal Amount);