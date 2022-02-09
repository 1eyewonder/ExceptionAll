namespace ExceptionAll.Client.Interfaces;

public interface ISerializerService
{
    ValueTask<T?> DeserializeContent<T>(HttpResponseMessage message, JsonSerializerOptions? options = null);
    StringContent SerializeContent<T>(T content, string mediaType, JsonSerializerOptions? options = null);
}