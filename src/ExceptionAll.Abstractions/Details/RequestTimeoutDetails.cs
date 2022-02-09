namespace ExceptionAll.Abstractions.Details;

public class RequestTimeoutDetails : IExceptionAllDetails
{
    public string Title => GetDetails().Title;
    public int StatusCode => GetDetails().StatusCode;
    public string Message { get; init; }
    public IReadOnlyDictionary<string, object>? ContextDetails { get; init; }
    public (int StatusCode, string Title) GetDetails()
    {
        return (408, "Request Timeout");
    }
}