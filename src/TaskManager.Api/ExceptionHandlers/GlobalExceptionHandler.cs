using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Exceptions;

namespace TaskManager.Api.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)

    {
        var problem = new ProblemDetails
        {
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            Detail = exception.Message,
        };

        switch (exception)
        {
            case ValidationException:
                problem.Title = "Validation failed";
                problem.Status = StatusCodes.Status400BadRequest;
                break;

            case EntityNotFoundException:
                problem.Title = "Entity not found";
                problem.Status = StatusCodes.Status404NotFound;
                break;

            default:
                problem.Title = "Internal Server Error";
                problem.Status = StatusCodes.Status500InternalServerError;
                break;
        }

        httpContext.Response.StatusCode = problem.Status ?? 500;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
