using ExceptionAll.Dtos;
using ExceptionAll.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace ExceptionAll.Details
{
    public class UnauthorizedDetails : ProblemDetails
    {
        public UnauthorizedDetails(ActionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            Title = title;
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status401Unauthorized;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            ProblemDetailsHelper.AddDefaultExtensionsFromContext(this, context, errors);
        }

        public UnauthorizedDetails(ExceptionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            Title = title;
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status401Unauthorized;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            ProblemDetailsHelper.AddDefaultExtensionsFromContext(this, context, errors);
        }
    }
}