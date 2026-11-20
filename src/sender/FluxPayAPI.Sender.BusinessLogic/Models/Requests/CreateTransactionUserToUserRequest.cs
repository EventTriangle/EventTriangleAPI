namespace FluxPayAPI.Sender.BusinessLogic.Models.Requests;

public record CreateTransactionUserToUserRequest(string ToUserId, decimal Amount, Guid? OperationId = null);