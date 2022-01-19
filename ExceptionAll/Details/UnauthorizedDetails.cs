namespace ExceptionAll.Details;

public class UnauthorizedDetails : IExceptionAllDetails
{
    public string Title => GetDetails().Title;
    public int StatusCode => GetDetails().StatusCode;
    public string Message { get; init; } = string.Empty;
    public IReadOnlyDictionary<string, object>? ContextDetails { get; init; }
    public (int StatusCode, string Title) GetDetails()
    {
        return (401, "Unauthorized");
    }
}