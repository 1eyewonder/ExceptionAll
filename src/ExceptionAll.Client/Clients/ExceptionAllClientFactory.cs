namespace ExceptionAll.Client;

public class ExceptionAllClientFactory : IExceptionAllClientFactory
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IValidationService _validation;
    private readonly JsonSerializerOptions _serializerOptions;

    public ExceptionAllClientFactory(
        IHttpClientFactory clientFactory,
        IValidationService validation,
        JsonSerializerOptions? serializerOptions = null)
    {
        _clientFactory     = clientFactory;
        _validation        = validation;
        _serializerOptions = serializerOptions ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public ExceptionAllClient CreateClient(string? clientName = null)
    {
        var client = clientName == null
            ? _clientFactory.CreateClient()
            : _clientFactory.CreateClient(clientName);

        return new ExceptionAllClient(
            client,
            _validation,
            _serializerOptions
        );
    }
}