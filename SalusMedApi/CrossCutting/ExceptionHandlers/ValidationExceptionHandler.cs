using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SalusMedApi.CrossCutting.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not ValidationException validationException)
            return false;

        var fields = validationException.Errors.Select(e => new
        {
            field = e.PropertyName,
            message = e.ErrorMessage,
        });

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation error",
            Type = "salusmed-api/errors/validation-error",
            Instance = context.Request.Path,
            Extensions = { ["timestamp"] = DateTime.UtcNow, ["fields"] = fields },
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
