namespace ExceptionAll.Client.Services;

public class SerializerService : ISerializerService
{
    /// <exception cref="JsonException"/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="NotSupportedException"/>
    public async ValueTask<T?> DeserializeContent<T>(HttpResponseMessage message, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<T>(await message.Content.ReadAsStringAsync(), options);
    }

    /// <exception cref="NotSupportedException"/>
    public StringContent SerializeContent<T>(T content, string mediaType, JsonSerializerOptions? options = null)
    {
        var stringContent = JsonSerializer.Serialize(content, options);
        return new StringContent(stringContent, Encoding.UTF8, mediaType);
    }
}