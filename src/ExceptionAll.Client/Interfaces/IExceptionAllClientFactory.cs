namespace ExceptionAll.Client.Interfaces;

public interface IExceptionAllClientFactory
{
    public ExceptionAllClient CreateClient();
    public ExceptionAllClient CreateClient(string clientName);
}