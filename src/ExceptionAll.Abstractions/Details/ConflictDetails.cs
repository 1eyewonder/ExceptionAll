namespace ExceptionAll.Abstractions.Details;

public class ConflictDetails : IExceptionAllDetails
{
    public string Title => GetDetails().Title;
    public int StatusCode => GetDetails().StatusCode;
    public string Message { get; init; }
    public IReadOnlyDictionary<string, object>? ContextDetails { get; init; }
    public (int StatusCode, string Title) GetDetails()
    {
        return (409, "Conflict");
    }
}