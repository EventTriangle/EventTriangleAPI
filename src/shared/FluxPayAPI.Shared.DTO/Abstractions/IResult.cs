using System.Net;

namespace FluxPayAPI.Shared.DTO.Abstractions;

public interface IResult<out TResponse, out TError>
{
    TResponse Response { get; }
    TError Error { get; }
    bool IsSuccess { get; }
    HttpStatusCode StatusCode { get; }
}