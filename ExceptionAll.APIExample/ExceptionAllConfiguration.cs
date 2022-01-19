using ExceptionAll.Interfaces;
using ExceptionAll.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ExceptionAll.APIExample;

public class ExceptionAllConfiguration : IExceptionAllConfiguration
{
    public List<IErrorResponse> ErrorResponses => new()
    {
        ErrorResponse
            .CreateErrorResponse()
            .WithTitle("Argument Null Exception")
            .WithStatusCode(500)
            .WithMessage("The developer goofed")
            .ForException<ArgumentNullException>()
            .WithLogAction((x, e) => x.LogDebug(e, "Oops I did it again"))
    };

    public Dictionary<string, Func<HttpContext, object>>? ContextConfiguration => new()
    {
        { "Path", x => x?.Request.Path.Value ?? string.Empty },
        { "Query", x => x.Request.QueryString.Value ?? string.Empty },
        { "TraceIdentifier", x => x?.TraceIdentifier ?? string.Empty },
        { "LocalIpAddress", x => x?.Connection.LocalIpAddress?.ToString() ?? string.Empty },
        {
            "CorrelationId",
            x => x.Request.Headers["x-correlation-id"]
                  .ToString()
        }
    };
}