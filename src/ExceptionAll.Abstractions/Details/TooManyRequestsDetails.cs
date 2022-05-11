namespace ExceptionAll.Abstractions;

public class TooManyRequestsDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (429, "Too Many Requests");
    }
}