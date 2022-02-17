namespace ExceptionAll.Client.Clients;

public class ExceptionAllClientFactory : IExceptionAllClientFactory
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ISerializerService _serializer;
    private readonly IValidationService _validation;

    public ExceptionAllClientFactory(
        IHttpClientFactory clientFactory,
        ISerializerService serializer,
        IValidationService validation)
    {
        _clientFactory = clientFactory;
        _serializer    = serializer;
        _validation    = validation;
    }

    public ExceptionAllClient CreateClient()
    {
        return new ExceptionAllClient(
            _clientFactory.CreateClient(),
            _serializer,
            _validation,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    public ExceptionAllClient CreateClient(string clientName)
    {
        return new ExceptionAllClient(
            _clientFactory.CreateClient(clientName),
            _serializer,
            _validation,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }
}