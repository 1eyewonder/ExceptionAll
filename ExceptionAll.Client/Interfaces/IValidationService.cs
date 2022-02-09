namespace ExceptionAll.Client.Interfaces;

public interface IValidationService
{
    ValueTask ValidateAsync(HttpResponseMessage message);
}