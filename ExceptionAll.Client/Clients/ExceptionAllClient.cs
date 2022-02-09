namespace ExceptionAll.Client.Clients;

/// <summary>
/// <inheritdoc cref="IExceptionAllClient"/>
/// </summary>
public class ExceptionAllClient : IExceptionAllClient
{
    private readonly HttpClient _httpClient;
    private readonly ISerializerService _serializer;
    private readonly IValidationService _validation;
    private readonly JsonSerializerOptions? _options;

    public ExceptionAllClient(
        HttpClient httpClient,
        ISerializerService serializer,
        IValidationService validation,
        JsonSerializerOptions? options = null)
    {
        _httpClient = httpClient;
        _serializer = serializer;
        _validation = validation;
        _options    = options;
    }

    ///<inheritdoc/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    /// <exception cref="InvalidOperationException"/>
    public async ValueTask<T?> GetContentAsync<T>(string relativeUrl, CancellationToken token = new())
    {
        var response = await _httpClient.GetAsync(relativeUrl, token);
        await _validation.ValidateAsync(response);

        return await _serializer.DeserializeContent<T>(response, _options);
    }

    ///<inheritdoc/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    /// <exception cref="InvalidOperationException"/>
    public async ValueTask<T?> DeleteContentAsync<T>(string relativeUrl, CancellationToken token = new())
    {
        var response = await _httpClient.DeleteAsync(relativeUrl, token);
        await _validation.ValidateAsync(response);

        return await _serializer.DeserializeContent<T>(response, _options);
    }

    ///<inheritdoc/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    /// <exception cref="InvalidOperationException"/>
    public async ValueTask<T?> PostContentAsync<T>(
        string relativeUrl, T content, string mediaType = "application/json", CancellationToken token = new())
    {
        var stringContent = _serializer.SerializeContent(content, mediaType, _options);
        var response      = await _httpClient.PostAsync(relativeUrl, stringContent, token);
        await _validation.ValidateAsync(response);

        return await _serializer.DeserializeContent<T>(response, _options);
    }

    ///<inheritdoc/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    /// <exception cref="InvalidOperationException"/>
    public async ValueTask<T?> PutContentAsync<T>(
        string relativeUrl, T content, string mediaType = "application/json", CancellationToken token = new())
    {
        var stringContent = _serializer.SerializeContent(content, mediaType, _options);
        var response      = await _httpClient.PutAsync(relativeUrl, stringContent, token);
        await _validation.ValidateAsync(response);

        return await _serializer.DeserializeContent<T>(response, _options);
    }
}