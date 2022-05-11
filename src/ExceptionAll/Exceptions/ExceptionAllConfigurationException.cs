namespace ExceptionAll;

public class ExceptionAllConfigurationException : Exception
{
    public ExceptionAllConfigurationException(string message) : base(message)
    {
        
    }

    public ExceptionAllConfigurationException(string message, Exception innerException) : base(message, innerException)
    {
        
        
    }
}