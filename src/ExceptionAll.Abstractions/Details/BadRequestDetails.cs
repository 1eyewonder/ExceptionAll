namespace ExceptionAll.Abstractions;

public class BadRequestDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (400, "Bad Request");
    }
}