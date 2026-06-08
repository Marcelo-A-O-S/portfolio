using System.Net;
using System.Text.Json;
using AuthService.Application.Exceptions;

namespace AuthService.API.Middleware
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        public ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)
        {
            this.next = _next;
            this.logger = _logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erro inesperado: {Message}", ex.Message);
                await HandleException(context, ex);
            }
        }
        private static Task HandleException(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case ValidationException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
            }
            var response = new
            {
                title = "Erro interno no servidor",
                detail = exception.Message,
                status = (int)statusCode
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.status;
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}