using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ExceptionAll.Interfaces
{
    public interface IActionResultService
    {
        ILogger<IActionResultService> Logger { get; }

        IActionResult GetBadRequestResponse(ActionContext context, string message = null);

        IActionResult GetErrorResponse(ExceptionContext context);

        IActionResult GetNotFoundResponse(ActionContext context, string message = null);

        IActionResult GetUnauthorizedResponse(ActionContext context, string message = null);
    }
}