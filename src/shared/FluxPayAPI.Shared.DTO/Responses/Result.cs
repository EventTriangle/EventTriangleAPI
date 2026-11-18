using System.Net;
using System.Text.Json.Serialization;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses.Errors;

namespace FluxPayAPI.Shared.DTO.Responses;

public class Result<TResponse> : IResult<TResponse, Error>
{
    [JsonPropertyName("response")]
    public TResponse Response { get; }

    [JsonPropertyName("error")]
    public Error Error { get; }

    [JsonPropertyName("is_success")]
    public bool IsSuccess { get; }

    [JsonPropertyName("status_code")]
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