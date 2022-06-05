namespace ExceptionAll;

/// <summary>
/// Service for creating and returning IResult error objects
/// </summary>
/// <remarks>
/// Used when operating with .NET Core middleware
/// </remarks>
public interface IResultService
{
    /// <summary>
    /// Create an error response for a middleware-caught exception
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    IResult GetErrorResponse(Exception exception, HttpContext context);
    
    /// <summary>
    /// Manually create an error response for developer caught errors
    /// </summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    /// <remarks>Intended for use in .NET Minimal APIs</remarks>
    IResult GetResponse<T>(
        HttpContext context, string? message = null, List<ErrorDetail>? errors = null)
        where T : IDetailBuilder, new();
}