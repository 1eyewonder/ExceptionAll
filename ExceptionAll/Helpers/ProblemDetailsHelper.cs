using ExceptionAll.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExceptionAll.Helpers
{
    public static class ProblemDetailsHelper
    {
        public static void AddDefaultExtensionsFromContext(this ProblemDetails details,
            ActionContext context,
            List<ErrorDetail> errors = null)
        {
            foreach (var (key, value) in GetExtensionsFromContext(context, errors))
            {
                details.Extensions.Add(key, value);
            }
        }

        public static void AddDefaultExtensionsFromContext(this ProblemDetails details,
            ExceptionContext context,
            List<ErrorDetail> errors = null)
        {
            foreach (var (key, value) in GetExtensionsFromContext(context, errors))
            {
                details.Extensions.Add(key, value);
            }
        }

        private static IDictionary<string, object> GetExtensionsFromContext(ActionContext context,
            List<ErrorDetail> errors = null)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Method", context.HttpContext.Request.Method },
                {"QueryString", context.HttpContext.Request.QueryString.Value },
                {"CorrelationId", context.HttpContext.Request.Headers["x-correlation-id"].ToString() },
                {"TraceId", context.HttpContext.TraceIdentifier }
            };

            if (errors is null || !errors.Any()) return dictionary;
            dictionary.Add("Errors", errors);
            return dictionary;
        }

        private static IDictionary<string, object> GetExtensionsFromContext(ExceptionContext context,
            List<ErrorDetail> errors = null)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Method", context.HttpContext.Request.Method },
                {"QueryString", context.HttpContext.Request.QueryString.Value },
                {"CorrelationId", context.HttpContext.Request.Headers["x-correlation-id"].ToString() },
                {"TraceId", context.HttpContext.TraceIdentifier }
            };

            if (errors is null || !errors.Any()) return dictionary;
            dictionary.Add("Errors", errors);
            return dictionary;
        }

        public static ConstructorInfo GetActionContextConstructor<T>()
            where T : ProblemDetails
        {
            try
            {
                return typeof(T).GetConstructor(new[]
                {
                    typeof(ActionContext),
                    typeof(string),
                    typeof(string),
                    typeof(List<ErrorDetail>)
                });
            }
            catch (Exception e)
            {
                throw new Exception($"Error creating constructor for type: {typeof(T)}", e);
            }
        }
    }
}