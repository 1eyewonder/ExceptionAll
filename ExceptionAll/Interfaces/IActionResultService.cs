using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ExceptionAll.Interfaces
{
    public interface IActionResultService
    {
        ILogger<IActionResultService> Logger { get; }

        IActionResult GetErrorResponse(ExceptionContext context);

        IActionResult GetResponse<T>(ActionContext context, int statusCode, string message = null) where T : ProblemDetails;
    }
}