namespace ExceptionAll.Abstractions;

public class ForbiddenDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (403, "Forbidden");
    }
}