using EventTriangleAPI.Shared.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Authorization.Presentation.Extensions;

public static class ResultExtension
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (!result.IsSuccess)
        {
            return new ObjectResult(new { result.Error.Message }) { StatusCode = StatusCodes.Status400BadRequest };
        }

        return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status200OK };
    }
}
