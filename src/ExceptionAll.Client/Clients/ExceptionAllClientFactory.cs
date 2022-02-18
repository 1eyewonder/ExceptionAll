namespace ExceptionAll.Client.Clients;

public class ExceptionAllClientFactory : IExceptionAllClientFactory
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IValidationService _validation;

    public ExceptionAllClientFactory(
        IHttpClientFactory clientFactory,
        IValidationService validation)
    {
        _clientFactory = clientFactory;
        _validation    = validation;
    }

    public ExceptionAllClient CreateClient(string? clientName = null)
    {
        var client = clientName == null 
            ? _clientFactory.CreateClient() 
            : _clientFactory.CreateClient(clientName);

        return new ExceptionAllClient(
            client,
            _validation,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }
}