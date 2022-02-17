namespace ExceptionAll.Client.Exceptions;

public class ExceptionAllException : Exception
{
    public HttpResponseMessage ResponseMessage { get; private set; }
    public ApiErrorDetails ErrorDetails { get; private set; }

    public ExceptionAllException(HttpResponseMessage responseMessage, ApiErrorDetails errorDetails) 
        : base(errorDetails.Message)
    {
        ResponseMessage = responseMessage;
        ErrorDetails    = errorDetails;
    }

    public ExceptionAllException(
        Exception innerException,
        HttpResponseMessage responseMessage,
        ApiErrorDetails errorDetails) : base(errorDetails.Message, innerException)
    {
        ResponseMessage = responseMessage;
        ErrorDetails    = errorDetails;
    }
}