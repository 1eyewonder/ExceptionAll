namespace ExceptionAll.Abstractions.Details;

public class BadRequestDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (400, "Bad Request");
    }
}