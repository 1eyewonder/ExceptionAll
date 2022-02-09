namespace ExceptionAll.Models;

public class ErrorResponse : IResponseTitle,
    IMessageCreation,
    IStatusIdentifier,
    IExceptionSelection,
    ILogAction
{
    public int StatusCode { get; private set; } = 500;
    public string ErrorTitle { get; private set; } = "Error";
    public string Message { get; private set; } = "There was an error encountered. If no errors are explicitly shown, please see logs for more details.";
    public Type ExceptionType { get; private set; } = typeof(Exception);
    public Action<ILogger<IActionResultService>, Exception>? LogAction { get; private set; }

    private ErrorResponse()
    {
    }

    public static ErrorResponse CreateErrorResponse()
    {
        return new ErrorResponse();
    }

    public IStatusIdentifier WithTitle(string title)
    {
        ErrorTitle = title;
        return this;
    }

    public IMessageCreation WithStatusCode(int statusCode)
    {
        StatusCode = statusCode;
        return this;
    }

    public IExceptionSelection WithMessage(string message)
    {
        Message = message;
        return this;
    }

    public ILogAction ForException<T>() where T : Exception
    {
        ExceptionType = typeof(T);
        return this;
    }

    ErrorResponse ILogAction.WithLogAction(Action<ILogger<IActionResultService>, Exception> action)
    {
        LogAction = action;
        return this;
    }
}