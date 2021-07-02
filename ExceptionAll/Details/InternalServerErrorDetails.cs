using System;
using ExceptionAll.Dtos;
using ExceptionAll.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace ExceptionAll.Details
{
    public class InternalServerErrorDetails : ProblemDetails
    {
        public InternalServerErrorDetails(ActionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            Title = string.IsNullOrEmpty(title) == false ? title :  "Internal Server Error";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status500InternalServerError;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }

        public InternalServerErrorDetails(ExceptionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            Title = string.IsNullOrEmpty(title) == false ? title :  "Internal Server Error";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status500InternalServerError;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }
    }
}