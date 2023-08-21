using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Shared.DTO.Responses;

public class ConflictError : Error
{
    public ConflictError(string message) : base(message) { }
}