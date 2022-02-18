namespace ExceptionAll.Client.Interfaces;

public interface IExceptionAllClientFactory
{
    public ExceptionAllClient CreateClient(string? clientName = null);
}