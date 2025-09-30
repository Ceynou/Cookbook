using Cookbook.SharedData;
using FluentValidation;

namespace Cookbook.API;

public class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IWebHostEnvironment env)
    : Exception
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "\r\nGlobally intercepted exception.\r\n");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is ValidationException fvex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            ErrorResponse response = new()
            {
                Error = "Validation errors occurred.",
                Details = string.Join("\r", fvex.Errors.Select(e => e.ErrorMessage))
            };
            return context.Response.WriteAsJsonAsync(response);
        }

        if (exception is ResourceNotFoundException rfex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            ErrorResponse response = new()
            {
                Error = "Resource could not be found.",
                Details = string.Join("\r", rfex.Message)
            };
            return context.Response.WriteAsJsonAsync(response);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            ErrorResponse response = new()
            {
                Error = "Internal error occurred.",
                Details = env.IsDevelopment()
                    ? $"{exception.GetType().Name} : {exception.Message}"
                    : "Please contact the system administrator."
            };
            return context.Response.WriteAsJsonAsync(response);
        }
    }

    private record ErrorResponse
    {
        public required string Error { get; set; }
        public required string Details { get; set; }
    }
}