using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ExceptionAll.Interfaces
{
    /// <summary>
    /// Service for creating and returning standard IActionResult error objects
    /// </summary>
    public interface IActionResultService
    {
        ILogger<IActionResultService> Logger { get; }

        /// <summary>
        /// Create an error response for a filter-caught exception
        /// </summary>
        /// <param name="context">Exception context</param>
        /// <returns></returns>
        IActionResult GetErrorResponse(ExceptionContext context);

        /// <summary>
        /// Manually create an error response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">ControllerContext</param>
        /// <param name="message">Optional developer message</param>
        /// <returns></returns>
        IActionResult GetResponse<T>(ActionContext context, string message = null) where T : ProblemDetails;
    }
}