using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace NMAS.WebApi.Host.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeaderKey = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Append(CorrelationIdHeaderKey, correlationId);
            }

            context.Response.OnStarting(() => {
                context.Response.Headers.Append(CorrelationIdHeaderKey, new[] { correlationId.ToString() });
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
