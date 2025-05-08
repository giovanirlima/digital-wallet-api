using CrossCutting.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int statusCode = StatusCodes.Status500InternalServerError;
        string title = "Server Error";
        string type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";

        if (exception is BaseException baseEx)
        {
            statusCode = baseEx.StatusCode;
            title = "Application Error";
            type = $"https://httpstatuses.io/{statusCode}";
        }

        ProblemDetails problemDetails = new()
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = exception.Message
        };

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}