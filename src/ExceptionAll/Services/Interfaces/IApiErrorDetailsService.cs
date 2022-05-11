namespace ExceptionAll;

public interface IApiErrorDetailsService
{
    /// <summary>
    /// Creates ApiErrorDetails based on the error response. If the response is null, creates default details
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    ApiErrorDetails GetErrorDetails(HttpContext context, Exception exception);
}