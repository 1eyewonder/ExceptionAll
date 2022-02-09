namespace ExceptionAll.Client.Services;

public class ValidationService : IValidationService
{
    private readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

    /// <exception cref="ExceptionAllException"/>
    public async ValueTask ValidateAsync(
        HttpResponseMessage message)
    {
        var content = await message.Content.ReadAsStringAsync();

        try
        {
            var details = JsonSerializer.Deserialize<ApiErrorDetails>(content, _options);
            if (details != null) throw new ExceptionAllException(message, details);
        }
        catch (Exception e) when (e.GetType() != typeof(ExceptionAllException))
        {
            // ignored
        }
    }
}