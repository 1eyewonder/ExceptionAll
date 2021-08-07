using ExceptionAll.Filters;
using ExceptionAll.Interfaces;
using ExceptionAll.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExceptionAll.Helpers
{
    public static class ServiceCollectionHelper
    {
        /// <summary>
        /// Inject all ExceptionAll related services into the IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExceptionAll(this IServiceCollection services)
        {
            services.AddSingleton<IErrorResponseService, ErrorResponseService>();
            services.AddSingleton<IActionResultService, ActionResultService>();

            return services.AddMvc(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            });
        }

        /// <summary>
        /// Add all error responses into the response collection
        /// </summary>
        /// <param name="service"></param>
        /// <param name="errorResponses"></param>
        public static void AddErrorResponses(this IErrorResponseService service,
            List<IErrorResponse> errorResponses)
        {
            if (errorResponses == null || !errorResponses.Any())
                throw new ArgumentNullException(nameof(errorResponses));

            foreach (var errorResponse in errorResponses)
            {
                service.AddErrorResponse(errorResponse);
            }
        }
    }
}
