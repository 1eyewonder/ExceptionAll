using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Helpers;
using ExceptionAll.Interfaces;
using ExceptionAll.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ExceptionAll.Services
{
    public class ActionResultService : IActionResultService
    {
        private readonly IErrorResponseService _errorResponseService;

        public ActionResultService(ILogger<IActionResultService> logger,
            IErrorResponseService errorResponseService)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _errorResponseService = errorResponseService ?? throw new ArgumentNullException(nameof(errorResponseService));
        }

        public ILogger<IActionResultService> Logger { get; init; }

        public IActionResult GetErrorResponse(ExceptionContext context)
        {
            ProblemDetails details;
            if (_errorResponseService.GetErrorResponses()
                .TryGetValue(context.Exception.GetType(),
                out ErrorResponse response))
            {
                new ErrorResponseValidator().ValidateAndThrow(response);
                var constructorInfo = response.DetailsType.GetConstructor(new Type[]
                {
                    typeof(ExceptionContext),
                    typeof(string),
                    typeof(string),
                    typeof(List<ErrorDetail>)
                });

                details = (ProblemDetails)constructorInfo.Invoke(new object[] { context, response.ErrorTitle, null, null });
                context.HttpContext.Response.StatusCode = (int)details.Status;

                if (response.LogAction is not null)
                {
                    response.LogAction(context.Exception);
                }
            }
            else
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                details = new InternalServerErrorDetails(context, "Internal Server Error");
                Logger.LogError(context.Exception, "Error encountered when accessing resource");
            }

            return new ObjectResult(details)
            {
                StatusCode = context.HttpContext.Response.StatusCode
            };
        }

        public IActionResult GetResponse<T>(ActionContext context, int statusCode, string message = null) where T : ProblemDetails
        {            
            T details;
            if (!typeof(T).IsSubclassOf(typeof(ProblemDetails)) &&
                typeof(T) == typeof(ProblemDetails))
            {
                var e = new Exception("ProblemDetails is not an acceptable type");
                Logger.LogError("ProblemDetails is not a valid type for this class. Please refer to documentation for assistance", e);
                throw e;
            }

            try
            {
                var constructorInfo = ProblemDetailsHelper.GetActionContextConstructor<T>();
                details = (T)constructorInfo.Invoke(new object[] { context, "Caught Exception", message ?? null, null });
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw new Exception($"Error when trying to invoke object constructor", e);
            }

            context.HttpContext.Response.StatusCode = (int)details.Status;
            Logger.LogTrace(message ?? nameof(T).Replace("Details", "").Trim());
            return new ObjectResult(details)
            {
                StatusCode = statusCode
            };
        }
    }
}