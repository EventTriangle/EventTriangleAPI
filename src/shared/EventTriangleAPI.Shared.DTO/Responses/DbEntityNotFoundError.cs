using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Shared.DTO.Responses;

public class DbEntityNotFoundError : Error
{
    public DbEntityNotFoundError(string message) : base(message) {}
}