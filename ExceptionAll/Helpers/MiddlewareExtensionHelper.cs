using ExceptionAll.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ExceptionAll.Helpers
{
    public static class MiddlewareExtensionHelper
    {
        public static IApplicationBuilder UseCorrelationIdMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}