using System.Net;
using System.Text.Json;

namespace PostService.API.Middleware
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
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Erro inesperado: {Message}", ex.Message);
            }
        }
        private static Task HandleException(HttpContext context, Exception exception)
        {
            var response = new
            {
              title = "Erro interno no servidor",
              detail = exception.Message,
              status = (int)HttpStatusCode.InternalServerError  
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.status;
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}