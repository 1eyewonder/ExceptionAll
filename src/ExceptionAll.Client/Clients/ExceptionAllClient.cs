using System.Net.Http.Json;

namespace ExceptionAll.Client.Clients;

/// <summary>
/// <inheritdoc cref="IExceptionAllClient"/>
/// </summary>
public class ExceptionAllClient : IExceptionAllClient
{
    private readonly HttpClient _httpClient;
    private readonly IValidationService _validation;
    private readonly JsonSerializerOptions? _options;

    public ExceptionAllClient(
        HttpClient httpClient,
        IValidationService validation,
        JsonSerializerOptions? options = null)
    {
        _httpClient = httpClient;
        _validation = validation;
        _options    = options;
    }

    ///<inheritdoc/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    /// <exception cref="InvalidOperationException"/>
    public async ValueTask<T?> GetContentAsync<T>(string relativeUrl, CancellationToken token = default)
    {
        var response = await _httpClient.GetAsync(relativeUrl, token);
        await _validation.ValidateAsync(response);
        return await response.Content.ReadFromJsonAsync<T>(_options, token);
    }

    ///<inheritdoc/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    /// <exception cref="InvalidOperationException"/>
    public async ValueTask<T?> DeleteContentAsync<T>(string relativeUrl, CancellationToken token = default)
    {
        var response = await _httpClient.DeleteAsync(relativeUrl, token);
        await _validation.ValidateAsync(response);
        return await response.Content.ReadFromJsonAsync<T>(_options, token);
    }

    ///<inheritdoc/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    /// <exception cref="InvalidOperationException"/>
    public async ValueTask<T?> PostContentAsync<T>(
        string relativeUrl, T content, CancellationToken token = new())
    {
        var response = await _httpClient.PostAsJsonAsync(relativeUrl, content, _options, token);
        await _validation.ValidateAsync(response);
        return await response.Content.ReadFromJsonAsync<T>(_options, token);
    }

    ///<inheritdoc/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    /// <exception cref="InvalidOperationException"/>
    public async ValueTask<T?> PutContentAsync<T>(
        string relativeUrl, T content, CancellationToken token = new())
    {
        var response = await _httpClient.PutAsJsonAsync(relativeUrl, content, _options, token);
        await _validation.ValidateAsync(response);
        return await response.Content.ReadFromJsonAsync<T>(_options, token);
    }
}