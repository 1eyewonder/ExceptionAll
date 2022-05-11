namespace ExceptionAll.Client;

public interface IValidationService
{
    ValueTask ValidateAsync(HttpResponseMessage message);
}