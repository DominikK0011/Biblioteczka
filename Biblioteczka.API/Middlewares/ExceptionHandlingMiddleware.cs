using System.Text.Json;

namespace Biblioteczka.API.Middlewares
{
    public sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                logger.LogError(ex, $"Error {errorId} for {context.Request.Method}, {context.Request.Path}");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new
                {
                    status = context.Response.StatusCode,
                    message = "Server Error",
                    errorId
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
