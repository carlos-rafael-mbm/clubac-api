using FluentValidation;
using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, response) = exception switch
        {
            UnauthorizedAccessException unauthorizedEx => ((int)HttpStatusCode.Unauthorized, new
            {
                code = "Unauthorized",
                message = unauthorizedEx.Message
            }),
            ValidationException validationEx => ((int)HttpStatusCode.BadRequest, (object)new
            {
                code = "ValidationError",
                message = "Validation failed for one or more fields.",
                details = validationEx.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                })
            }),
            KeyNotFoundException notFoundEx => ((int)HttpStatusCode.NotFound, new
            {
                code = "NotFound",
                message = notFoundEx.Message
            }),
            Exception ex => ((int)HttpStatusCode.InternalServerError, new
            {
                code = "InternalServerError",
                message = ex.Message
            })
        };

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
