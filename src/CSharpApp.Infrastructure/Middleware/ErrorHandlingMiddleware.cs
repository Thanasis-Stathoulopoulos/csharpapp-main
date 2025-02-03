using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace CSharpApp.Infrastructure.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP Request error occurred");
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadGateway);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation error occurred");
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode code)
    {
        var response = new ProblemDetails
        {
            Status = (int)code,
            Title = GetTitle(code),
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static string GetTitle(HttpStatusCode code) => code switch
    {
        HttpStatusCode.BadRequest => "Bad Request",
        HttpStatusCode.BadGateway => "External Service Error",
        _ => "Server Error"
    };
}