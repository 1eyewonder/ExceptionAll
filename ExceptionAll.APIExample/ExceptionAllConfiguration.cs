using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ExceptionAll.APIExample
{
    public static class ExceptionAllConfiguration
    {
        public static List<IErrorResponse> GetErrorResponses()
        {
            return new List<IErrorResponse>()
            {
                ErrorResponse
                    .CreateErrorResponse()
                    .WithTitle("Bad Request - Fluent Validation")
                    .ForException<ValidationException>()
                    .WithReturnType<BadRequestDetails>()
                    .WithLogAction((x, e) => x.LogError(e, "Something bad happened")),

                ErrorResponse
                    .CreateErrorResponse()
                    .WithTitle("Bad Request")
                    .ForException<ArgumentNullException>()
                    .WithReturnType<BadRequestDetails>()
                    .WithLogAction((x, e) => x.LogError(e, "Something bad happened"))
            };
        }
    }
}
