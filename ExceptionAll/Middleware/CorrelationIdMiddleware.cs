using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ExceptionAll.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("x-correlation-id"))
            {
                context.Request.Headers.Add("x-correlation-id", Guid.NewGuid().ToString("N"));
            }

            await _next.Invoke(context);
        }
    }
}