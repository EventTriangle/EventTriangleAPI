using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Shared.DTO.Responses;

public class BadRequestError : Error
{
    public BadRequestError(string message) : base(message) { }
}