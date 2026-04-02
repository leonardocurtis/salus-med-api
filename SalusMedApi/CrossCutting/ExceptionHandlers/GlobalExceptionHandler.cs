using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.CrossCutting.Exceptions;

namespace SalusMedApi.CrossCutting.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var (statusCode, type) = exception switch
        {
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "unauthorized"),
            ResourceNotFoundException => (StatusCodes.Status404NotFound, "resource-not-found"),
            ForbiddenException => (StatusCodes.Status403Forbidden, "access-denied"),
            _ => (StatusCodes.Status500InternalServerError, "internal-error"),
        };

        var problem = BuildProblem(context, exception, statusCode, type);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static ProblemDetails BuildProblem(
        HttpContext context,
        Exception exception,
        int statusCode,
        string type
    )
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = exception.Message,
            Type = $"salusmed-Api/errors/{type}",
            Instance = context.Request.Path,
            Extensions = { ["timestamp"] = DateTime.UtcNow },
        };
    }

    private static string GetTitle(int statusCode) =>
        statusCode switch
        {
            400 => "Bad Request",
            404 => "Not Found",
            _ => "Internal Server Error",
        };
}
