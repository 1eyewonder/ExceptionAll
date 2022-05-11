namespace ExceptionAll;

public interface IContextConfigurationService
{
    /// <summary>
    /// Reads data from the HttpContext and maps it according to the configuration provided
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<string, object>? GetContextDetails(HttpContext context, List<ErrorDetail>? errors = null);

    /// <summary>
    /// Returns the current configuration in use
    /// </summary>
    /// <returns></returns>
    IExceptionAllConfiguration GetConfiguration();
}