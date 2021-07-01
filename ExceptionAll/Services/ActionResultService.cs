using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Interfaces;
using ExceptionAll.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExceptionAll.Services
{
    public class ActionResultService : IActionResultService
    {
        public ILogger<IActionResultService> Logger { get; init; }
        private readonly IErrorResponseService _errorResponseService;

        public ActionResultService(ILogger<IActionResultService> logger,
            IErrorResponseService errorResponseService)
        {
            Logger = logger;
            _errorResponseService = errorResponseService;
        }

        public IActionResult GetBadRequestResponse(ActionContext context, string message = null)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            BadRequestDetails details = new(context, "Bad Request", message);
            Logger.LogDebug(message ?? "Bad request");

            return new BadRequestObjectResult(details);
        }

        public IActionResult GetErrorResponse(ExceptionContext context)
        {
            ProblemDetails details;
            if (_errorResponseService.GetErrorResponses()
                .TryGetValue(context.Exception.GetType(), 
                out ErrorResponse response))
            {
                context.HttpContext.Response.StatusCode = response.StatusCode;
                var validator = new ErrorResponseValidator();
                validator.ValidateAndThrow(response);

                var constructorInfo = response.DetailsType.GetConstructor(new Type[]
                {
                    typeof(ExceptionContext), 
                    typeof(string), 
                    typeof(string), 
                    typeof(List<ErrorDetail>)
                });

                details = (ProblemDetails)constructorInfo.Invoke(new object[] { context, response.ErrorTitle, null, null });

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

        public IActionResult GetNotFoundResponse(ActionContext context, string message = null)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            NotFoundDetails details = new(context, "Not Found", message);
            Logger.LogDebug(message ?? "Not Found");

            return new NotFoundObjectResult(details);
        }

        public IActionResult GetUnauthorizedResponse(ActionContext context, string message = null)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            UnauthorizedDetails details = new(context, "Unauthorized", message);
            Logger.LogDebug(message ?? "Unauthorized");

            return new UnauthorizedObjectResult(details);
        }
    }
}