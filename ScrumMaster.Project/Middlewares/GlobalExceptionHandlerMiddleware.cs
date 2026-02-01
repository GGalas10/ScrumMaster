using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ScrumMaster.Project.Infrastructure.CustomExceptions;
using System;
using System.Threading.Tasks;

namespace ScrumMaster.Project.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception in Project API");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var message = exception.Message;
            var statusCode = exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                RoleException => StatusCodes.Status403Forbidden,
                UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                // Hide internal details for 500 responses
                message = "Something_Went_Wrong";
            }

            context.Response.StatusCode = statusCode;

            var payload = new
            {
                message,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(payload);
        }
    }
}