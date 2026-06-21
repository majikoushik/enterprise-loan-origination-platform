using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedKernel.Exceptions;

namespace Observability;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, CorrelationIdProvider correlationIdProvider) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var correlationId = correlationIdProvider.Get();

        // Safe logging mapping
        var (statusCode, title, detail) = MapException(exception);

        // Do not leak stack traces unless in local development, but to be absolutely safe, we never leak it in problem details.
        logger.LogError(exception, "An error occurred while processing the request. Correlation ID: {CorrelationId}. Type: {ExceptionType}, Detail: {Detail}", correlationId, exception.GetType().Name, detail);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };
        problemDetails.Extensions["correlationId"] = correlationId;

        // If validation exception, add errors
        if (exception is ValidationException validationException)
        {
            problemDetails.Type = "https://example.com/problems/validation-error";
            problemDetails.Extensions["errors"] = validationException.Errors;
        }
        else if (statusCode == StatusCodes.Status500InternalServerError)
        {
            problemDetails.Type = "https://example.com/problems/internal-server-error";
            problemDetails.Detail = "An unexpected error occurred. Please contact support."; // Mask actual server error
        }
        else
        {
            problemDetails.Type = $"https://example.com/problems/error-{statusCode}";
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int StatusCode, string Title, string Detail) MapException(Exception exception)
    {
        return exception switch
        {
            ValidationException ve => (StatusCodes.Status400BadRequest, "Validation failed", "One or more validation errors occurred."),
            NotFoundException ne => (StatusCodes.Status404NotFound, "Resource not found", ne.Message),
            ConflictException ce => (StatusCodes.Status409Conflict, "Resource conflict", ce.Message),
            DomainException de => (StatusCodes.Status400BadRequest, "Business rule violation", de.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error", exception.Message)
        };
    }
}
