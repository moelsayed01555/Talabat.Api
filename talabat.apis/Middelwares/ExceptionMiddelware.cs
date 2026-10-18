using System.Text.Json;
using talabat.apis.Errors;

namespace talabat.apis.Middelwares
{
    public class ExceptionMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddelware> _logger;
        private readonly IHostEnvironment _environment;

        // handle server error excption by convension

        public ExceptionMiddelware(RequestDelegate next, ILogger<ExceptionMiddelware> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

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
                context.Response.StatusCode = 500;
                var response = _environment.IsDevelopment()
                    ? new ApiErrorResponse(500, ex.Message)         // in development environment, return the exception message
                    : new ApiErrorResponse(500);                    // in production environment, return a generic message
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
