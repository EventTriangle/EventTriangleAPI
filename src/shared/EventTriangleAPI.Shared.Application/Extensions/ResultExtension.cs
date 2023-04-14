using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Shared.Application.Extensions;

public static class ResultExtension
{
    public static IActionResult ToActionResult<T>(this IResult<T, Error>result)
    {
        var objectResult = GenerateFromResponse(result);

        return objectResult;
    }

    private static ObjectResult GenerateFromResponse<T>(IResult<T, Error> response)
    {
        var objectResult = new ObjectResult(response);
        var statusCode = (int?)response.StatusCode;
        objectResult.StatusCode = statusCode;

        return objectResult;
    }
}