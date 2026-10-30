using System.Net;
using System.Text.Json;
using KosKu.Application.Common;

namespace KosKu.Api.Middleware;

/// <summary>
/// Global Exception Middleware — menangkap semua unhandled exception dan mengubahnya
/// menjadi format ApiResponse yang konsisten.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, response) = exception switch
        {
            NotFoundException notFound => (
                HttpStatusCode.NotFound,
                ApiResponse<object>.Fail(notFound.Message)
            ),
            ValidationException validation => (
                HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail("Validation Error", validation.Errors)
            ),
            ForbiddenException forbidden => (
                HttpStatusCode.Forbidden,
                ApiResponse<object>.Fail(forbidden.Message)
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                ApiResponse<object>.Fail("Unauthorized. Silakan login terlebih dahulu.")
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail("Terjadi kesalahan internal server. Silakan coba lagi.")
            )
        };

        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
