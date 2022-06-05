namespace ExceptionAll.Abstractions;

public class ApiErrorDetails : IApiErrorDetails
{
    public string Title { get; init; } = "ExceptionAll default error";
    public int StatusCode { get; init; } = 500;
    public string Message { get; init; } = "There was an error encountered";
    public IReadOnlyDictionary<string, object>? ContextDetails { get; init; }
}
