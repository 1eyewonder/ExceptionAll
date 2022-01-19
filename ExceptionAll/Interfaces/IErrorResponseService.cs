namespace ExceptionAll.Interfaces;

/// <summary>
/// Service for managing global error responses
/// </summary>
public interface IErrorResponseService
{
    /// <summary>
    /// Returns the error response created for the given exception type
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    IErrorResponse? GetErrorResponse<T>(T exception) where T : Exception;
}