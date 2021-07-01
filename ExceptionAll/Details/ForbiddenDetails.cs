using ExceptionAll.Dtos;
using ExceptionAll.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace ExceptionAll.Details
{
    public class ForbiddenDetails : ProblemDetails
    {
        public ForbiddenDetails(ActionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            Title = title;
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status403Forbidden;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            ProblemDetailsHelper.AddDefaultExtensionsFromContext(this, context, errors);
        }

        public ForbiddenDetails(ExceptionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            Title = title;
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status403Forbidden;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            ProblemDetailsHelper.AddDefaultExtensionsFromContext(this, context, errors);
        }
    }
}
