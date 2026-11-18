using FluxPayAPI.Shared.DTO.Responses.Errors;

namespace FluxPayAPI.Shared.DTO.Responses;

public class DbEntityNotFoundError : Error
{
    public DbEntityNotFoundError(string message) : base(message) {}
}