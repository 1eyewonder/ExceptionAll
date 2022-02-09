namespace ExceptionAll.Client.Exceptions;

public class ExceptionAllException : Exception
{
    public HttpResponseMessage ResponseMessage { get; private set; }
    public ApiErrorDetails ErrorDetails { get; private set; }

    public ExceptionAllException(HttpResponseMessage responseMessage, ApiErrorDetails errorDetails)
    {
        ResponseMessage = responseMessage;
        ErrorDetails    = errorDetails;
    }
}