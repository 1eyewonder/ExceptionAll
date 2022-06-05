namespace ExceptionAll.Abstractions;

public class ConflictDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (409, "Conflict");
    }
}