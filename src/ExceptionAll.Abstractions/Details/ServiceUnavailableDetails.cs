namespace ExceptionAll.Abstractions;

public class ServiceUnavailableDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (503, "Service Unavailable");
    }
}