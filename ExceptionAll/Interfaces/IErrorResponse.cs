using ExceptionAll.Models;

namespace ExceptionAll.Interfaces;

/// <summary>
/// Constructed response for a given exception type
/// </summary>
public interface IErrorResponse
{
    int StatusCode { get; }
    string ErrorTitle { get; }
    string Message { get; }
    Type ExceptionType { get; }
    Action<ILogger<IActionResultService>, Exception>? LogAction { get; }
}

public interface IResponseTitle : IErrorResponse
{
    /// <summary>
    /// Creates title for the returned error response object
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    public IStatusIdentifier WithTitle(string title);
}

public interface IStatusIdentifier : IErrorResponse
{
    public IMessageCreation WithStatusCode(int statusCode);
}

public interface IMessageCreation : IErrorResponse
{
    public IExceptionSelection WithMessage(string message);
}

public interface IExceptionSelection : IErrorResponse
{
    /// <summary>
    /// Type of error that will trigger our filter
    /// </summary>
    /// <typeparam name="T">Exception type</typeparam>
    /// <returns></returns>
    public ILogAction ForException<T>() where T : Exception;
}

public interface ILogAction : IErrorResponse
{
    /// <summary>
    /// Logging action that will occur if an exception is caught
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public ErrorResponse WithLogAction(Action<ILogger<IActionResultService>, Exception> action);
}