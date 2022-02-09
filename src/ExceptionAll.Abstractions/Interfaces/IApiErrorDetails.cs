namespace ExceptionAll.Abstractions.Interfaces;

public interface IApiErrorDetails
{
    string Title { get; }
    int StatusCode { get; }
    string Message { get; init; }
    public IReadOnlyDictionary<string, object>? ContextDetails { get; init; }
}