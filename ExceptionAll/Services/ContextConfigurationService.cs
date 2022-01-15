using ExceptionAll.Models;

namespace ExceptionAll.Services;

public class ContextConfigurationService : IContextConfigurationService
{
    private Dictionary<string, Func<HttpContext, object>>? _contextMappings;

    public void Configure(Dictionary<string, Func<HttpContext, object>> configuration)
    {
        _contextMappings = new Dictionary<string, Func<HttpContext, object>>(configuration);
    }

    public IReadOnlyDictionary<string, object>? GetContextDetails(HttpContext context, List<ErrorDetail>? errors = null)
    {
        var dictionary = _contextMappings?.ToDictionary(x => x.Key, x => x.Value(context));

        if (errors is null || !errors.Any()) return dictionary;

        if (dictionary != null) dictionary.Add("Errors", errors);
        else dictionary = new Dictionary<string, object> { { "Errors", errors } };

        return dictionary;
    }
}