using System;
using ExceptionAll.Dtos;
using ExceptionAll.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace ExceptionAll.Details
{
    public class BadRequestDetails : ProblemDetails
    {
        public BadRequestDetails(ActionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            Title = string.IsNullOrEmpty(title) == false ? title :  "Bad Request";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status400BadRequest;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }

        public BadRequestDetails(ExceptionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            Title = string.IsNullOrEmpty(title) == false ? title :  "Bad Request";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status400BadRequest;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }
    }
}