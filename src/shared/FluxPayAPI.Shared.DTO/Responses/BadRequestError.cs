using FluxPayAPI.Shared.DTO.Responses.Errors;

namespace FluxPayAPI.Shared.DTO.Responses;

public class BadRequestError : Error
{
    public BadRequestError(string message) : base(message) { }
}