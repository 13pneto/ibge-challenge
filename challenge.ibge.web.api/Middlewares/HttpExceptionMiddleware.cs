using System.Net;
using System.Text.Json;

namespace challenge.ibge.web.api.Middlewares;

public class HttpExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpExceptionMiddleware> _logger;

    public HttpExceptionMiddleware(RequestDelegate next, ILogger<HttpExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Middleware used to intercept requests and return an InternalErrorException with the exception message
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorBody = new InternalErrorException("Internal server error", ex.Message);

            var errorBodyJson = JsonSerializer.Serialize(errorBody);
            await context.Response.WriteAsync(errorBodyJson);
        }
    }
}