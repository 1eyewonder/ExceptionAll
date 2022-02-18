namespace ExceptionAll.Abstractions.Details;

public class UnauthorizedDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (401, "Unauthorized");
    }
}