namespace ExceptionAll;

/// <summary>
/// Service for creating and returning IActionResult error objects
/// </summary>
/// <remarks>
/// Used when operating with .NET Core MVC
/// </remarks>
public interface IActionResultService
{
    /// <summary>
    /// Create an error response for a filter-caught exception
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