namespace ExceptionAll.Abstractions;

public class BaseDetails : IExceptionAllDetails
{
    public string Title => GetDetails().Title;
    public int StatusCode => GetDetails().StatusCode;
    public string Message { get; init; } = string.Empty;
    public IReadOnlyDictionary<string, object>? ContextDetails { get; init; }
    public virtual (int StatusCode, string Title) GetDetails()
    {
        return (500, "Default Details");
    }
}