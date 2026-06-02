using System.Net;
using System.Text.Json;
using m_motors_API.Services;

namespace m_motors_API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(
            HttpContext context,
            ILogService logService)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await logService.LogErrorAsync(
                    "Erreur serveur",
                    ex,
                    context.Request.Path,
                    context.Request.Method,
                    context.User?.Identity?.Name
                );
                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType =
                    "application/json";

                var response = new
                {
                    message = "Erreur interne serveur"
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}