namespace SmartWings.API.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Text.Json;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public  ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            _logger.LogError(ex, "Exception caught and handled in middleware.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                status = context.Response.StatusCode,
                error = statusCode.ToString(),
                message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred.",
                stackTrace = _env.IsDevelopment() ? ex.StackTrace : null,
                path = context.Request.Path,
                method = context.Request.Method,
                timestamp = DateTime.UtcNow.ToString("o"),
                traceId = context.TraceIdentifier
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(errorResponse, jsonOptions);
            await context.Response.WriteAsync(json);
        }
    }
}
