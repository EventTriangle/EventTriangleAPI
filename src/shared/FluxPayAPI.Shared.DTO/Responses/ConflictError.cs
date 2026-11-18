using FluxPayAPI.Shared.DTO.Responses.Errors;

namespace FluxPayAPI.Shared.DTO.Responses;

public class ConflictError : Error
{
    public ConflictError(string message) : base(message) { }
}