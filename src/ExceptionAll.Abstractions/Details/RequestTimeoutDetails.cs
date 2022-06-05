namespace ExceptionAll.Abstractions;

public class RequestTimeoutDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (408, "Request Timeout");
    }
}