using System.Data;

namespace ExceptionAll;

public class ContextConfigurationService : IContextConfigurationService
{
    private readonly IExceptionAllConfiguration _configuration;
    private readonly Dictionary<string, Func<HttpContext, object>>? _contextMappings;

    public ContextConfigurationService(IExceptionAllConfiguration configuration)
    {
        _configuration = configuration;
        if (configuration.ContextConfiguration is not null)
            _contextMappings = new(configuration.ContextConfiguration);
    }

    public IReadOnlyDictionary<string, object>? GetContextDetails(HttpContext context, List<ErrorDetail>? errors = null)
    {
        var dictionary = _contextMappings?.ToDictionary(x => x.Key, x => x.Value(context));

        if (errors is null || !errors.Any()) return dictionary;

        if (dictionary is not null && dictionary.ContainsKey("Errors"))
            throw new ExceptionAllConfigurationException(
                "The key 'Errors' is reserved for ExceptionAll. Please alter your configuration to use another key"
            );
        
        if (dictionary is not null) dictionary.Add("Errors", errors);
        else dictionary = new Dictionary<string, object> { { "Errors", errors } };

        return dictionary;
    }

    public IExceptionAllConfiguration GetConfiguration()
    {
        return _configuration;
    }
}