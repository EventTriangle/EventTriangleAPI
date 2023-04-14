using System.Net;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Shared.DTO.Responses;

public class Result<TResponse> : IResult<TResponse, Error>
{
    public TResponse Response { get; }
    public Error Error { get; }
    public bool IsSuccess { get; }
    public HttpStatusCode StatusCode { get; }

    public Result(TResponse response)
    {
        Response = response;
        IsSuccess = true;
        StatusCode = HttpStatusCode.OK;
    }

    public Result(Error error)
    {
        Error = error;
        IsSuccess = false;
        StatusCode = HttpStatusCode.BadRequest;
    }
}