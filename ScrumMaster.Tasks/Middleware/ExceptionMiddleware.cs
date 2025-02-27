using ScrumMaster.Tasks.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace ScrumMaster.Tasks.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
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
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new { error = exception.Message };

            switch (exception)
            {
                case BadRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case UnauthorizedException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
