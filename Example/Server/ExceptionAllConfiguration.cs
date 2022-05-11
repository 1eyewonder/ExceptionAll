using ExceptionAll;
using Microsoft.AspNetCore.Mvc;

namespace Example.Server;

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
        { "Path", x => x.Request.Path.Value ?? string.Empty },
        { "Query", x => x.Request.QueryString.Value ?? string.Empty },
        { "TraceIdentifier", x => x.TraceIdentifier },
        { "LocalIpAddress", x => x.Connection.LocalIpAddress?.ToString() ?? string.Empty },
        {
            "CorrelationId",
            x => x.Request.Headers["x-correlation-id"]
                  .ToString()
        }
    };

    /// <inheritdoc cref="IExceptionAllConfiguration.ValidationLoggingAction"/>
    public Action<ILogger, ActionContext>? ValidationLoggingAction => (logger, context) =>
    {
        foreach (var (key, value) in context.ModelState)
        {
            foreach (var error in value.Errors)
            {
                logger.LogWarning(error.Exception, "Error with {Key}: {Message}", key, error.ErrorMessage);
            }
        }
    };
}