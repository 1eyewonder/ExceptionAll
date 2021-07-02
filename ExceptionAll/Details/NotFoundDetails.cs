using ExceptionAll.Dtos;
using ExceptionAll.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace ExceptionAll.Details
{
    public class NotFoundDetails : ProblemDetails
    {
        public NotFoundDetails(ActionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            Title = string.IsNullOrEmpty(title) == false ? title :  "Not Found";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status404NotFound;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }

        public NotFoundDetails(ExceptionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            Title = string.IsNullOrEmpty(title) == false ? title :  "Not Found";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status404NotFound;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }
    }
}