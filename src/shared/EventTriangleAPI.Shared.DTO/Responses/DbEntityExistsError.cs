using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Shared.DTO.Responses;

public class DbEntityExistsError : Error
{
    public DbEntityExistsError(string message) : base(message) { }
}