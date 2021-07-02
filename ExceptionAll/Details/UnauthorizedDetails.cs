﻿using ExceptionAll.Dtos;
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
            Title = string.IsNullOrEmpty(title) == false ? title :  "Unauthorized";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status401Unauthorized;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }

        public UnauthorizedDetails(ExceptionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            Title = string.IsNullOrEmpty(title) == false ? title :  "Unauthorized";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status401Unauthorized;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }
    }
}