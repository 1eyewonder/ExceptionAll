namespace ExceptionAll.Abstractions;

public class UnauthorizedDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (401, "Unauthorized");
    }
}