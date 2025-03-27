using System;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace macro_tracker_web_service.Middleware
{
    public class MacroExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MacroExceptionHandler> _logger;

        public MacroExceptionHandler(RequestDelegate next, ILogger<MacroExceptionHandler> logger)
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
                // Log detailed information about the exception
                _logger.LogError(ex,
                    "An unhandled exception occurred. " +
                    "Path: {Path}, " +
                    "Method: {Method}, " +
                    "Exception Type: {ExceptionType}",
                    context.Request.Path,
                    context.Request.Method,
                    ex.GetType().Name);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Determine status code based on exception type
            var statusCode = exception switch
            {
                DbUpdateException _ => HttpStatusCode.Conflict,
                ArgumentException _ => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = (int)statusCode,
                Message = exception.Message,
            };

            var result = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(result);
        }
    }

    public static class MacroExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseMacroExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MacroExceptionHandler>();
        }
    }
}