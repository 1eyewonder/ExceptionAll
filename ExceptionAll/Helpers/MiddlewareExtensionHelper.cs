using ExceptionAll.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ExceptionAll.Helpers
{
    public static class MiddlewareExtensionHelper
    {
        /// <summary>
        /// Add 'x-correlation-id' header to all incoming http requests
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorrelationIdMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}