namespace ExceptionAll.Interfaces;

public interface IExceptionAllConfiguration
{
    /// <summary>
    /// Collection of error responses
    /// </summary>
    List<IErrorResponse> ErrorResponses { get; }

    /// <summary>
    /// Maps user defined property names to accessible members of the HttpContext
    /// </summary>
    Dictionary<string, Func<HttpContext, object>>? ContextConfiguration { get; }
}