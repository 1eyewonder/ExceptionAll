using ExceptionAll.Models;

namespace ExceptionAll.Interfaces;

public interface IContextConfigurationService
{
    /// <summary>
    /// Allows the user to configure additional properties shown in the response which can be derived from the HttpContext
    /// </summary>
    /// <param name="configuration"></param>
    void Configure(Dictionary<string, Func<HttpContext, object>> configuration);

    /// <summary>
    /// Reads data from the HttpContext and maps it according to the configuration provided
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<string, object>? GetContextDetails(HttpContext context, List<ErrorDetail>? errors = null);
}