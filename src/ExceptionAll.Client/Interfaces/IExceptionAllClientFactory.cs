namespace ExceptionAll.Client;

public interface IExceptionAllClientFactory
{
    public ExceptionAllClient CreateClient(string? clientName = null);
}