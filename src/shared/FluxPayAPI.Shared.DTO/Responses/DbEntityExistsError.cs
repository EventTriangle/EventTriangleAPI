using FluxPayAPI.Shared.DTO.Responses.Errors;

namespace FluxPayAPI.Shared.DTO.Responses;

public class DbEntityExistsError : Error
{
    public DbEntityExistsError(string message) : base(message) { }
}