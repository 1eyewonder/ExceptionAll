namespace ExceptionAll.Client;

public class ValidationService : IValidationService
{
    private readonly JsonSerializerOptions _options;

    public ValidationService(JsonSerializerOptions? options = null)
    {
        _options = options ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    /// <exception cref="ExceptionAllException" />
    public async ValueTask ValidateAsync(
        HttpResponseMessage message)
    {
        try
        {
            var content = await message.Content.ReadAsStringAsync();
            var details = JsonSerializer.Deserialize<ApiErrorDetails>(content, _options);

            if (details != null) throw new ExceptionAllException(message, details);
        }
        catch (Exception e) when (e.GetType() != typeof(ExceptionAllException))
        {
            // ignored
        }
    }
}