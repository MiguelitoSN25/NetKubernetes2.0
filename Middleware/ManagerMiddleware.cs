using Newtonsoft.Json;
using System.Net;

namespace NetKubernetes2._0.Middleware
{
    public class ManagerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ManagerMiddleware> _logger;

        public ManagerMiddleware(RequestDelegate next, ILogger<ManagerMiddleware?> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ManagerExceptionAsync(context, ex, _logger);
            }
        }

        private async Task ManagerExceptionAsync(HttpContext context, Exception exception, ILogger<ManagerMiddleware> logger)
        {
            object? errores = null;

            switch(exception)
            {
                case MiddlewareException me:
                logger.LogError(exception, "Middleware Error");
                    errores = me.Errores;
                    context.Response.StatusCode = (int)me.Codigo;
                    break;
                case Exception ex:
                    logger.LogError(ex, "Error de Servidor");
                    errores = string.IsNullOrEmpty(ex.Message) ? "Error" : ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            var resultados = string.Empty;
            if (errores != null)
            {
                resultados = JsonConvert.SerializeObject(new { errores });
            }
            await context.Response.WriteAsync(resultados);

        }
    }
}
