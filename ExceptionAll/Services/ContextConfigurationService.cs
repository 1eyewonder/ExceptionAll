namespace ExceptionAll.Services;

public class ContextConfigurationService : IContextConfigurationService
{
    private readonly Dictionary<string, Func<HttpContext, object>>? _contextMappings;

    public ContextConfigurationService(IExceptionAllConfiguration configuration)
    {
        if (configuration.ContextConfiguration is not null)
            _contextMappings = new(configuration.ContextConfiguration);
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