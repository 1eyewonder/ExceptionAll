using ExceptionAll.Models;

namespace ExceptionAll.Interfaces;

/// <summary>
/// Service for creating and returning standard IActionResult error objects
/// </summary>
public interface IActionResultService
{
    /// <summary>
    /// Create an error response for a filter-caught exceptions
    /// </summary>
    /// <param name="context">Exception context</param>
    /// <returns></returns>
    IActionResult GetErrorResponse(ExceptionContext context);

    /// <summary>
    /// Manually create an error response for developer caught errors
    /// </summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    IActionResult GetResponse<T>(
        ActionContext context, string? message = null, List<ErrorDetail>? errors = null)
        where T : IDetailBuilder, new();
}