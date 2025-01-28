using EventTriangleAPI.Shared.DTO.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventTriangleAPI.Sender.Presentation.Filters;

public class HttpResponseExceptionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            context.Result = new JsonResult(new Result<object>(new BadRequestError(validationException.Message)))
            {
                StatusCode = StatusCodes.Status400BadRequest,
            };
            
            context.ExceptionHandled = true;
        }
    }
}