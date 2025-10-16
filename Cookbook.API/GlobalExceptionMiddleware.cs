using Cookbook.SharedData.Exceptions;
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

        switch (exception)
        {
            case ValidationException fvex:
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                ErrorResponse response = new()
                {
                    Error = "Validation errors occurred.",
                    Details = string.Join("\r", fvex.Errors.Select(e => e.ErrorMessage))
                };
                return context.Response.WriteAsJsonAsync(response);
            }
            case InvalidCredentialsException icex:
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                ErrorResponse response = new()
                {
                    Error = "Invalid credentials.",
                    Details = string.Join("\r", icex.Message)
                };
                return context.Response.WriteAsJsonAsync(response);
            }
            case ResourceNotFoundException rfex:
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                ErrorResponse response = new()
                {
                    Error = "Resource could not be found.",
                    Details = string.Join("\r", rfex.Message)
                };
                return context.Response.WriteAsJsonAsync(response);
            }
            case DuplicatePropertyException dpex:
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                ErrorResponse response = new()
                {
                    Error = "Property is already taken.",
                    Details = string.Join("\r", dpex.Message)
                };
                return context.Response.WriteAsJsonAsync(response);
            }
            default:
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
    }

    private record ErrorResponse
    {
        public required string Error { get; set; }
        public required string Details { get; set; }
    }
}