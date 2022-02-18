namespace ExceptionAll.Abstractions.Details;

public class BadGatewayDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (502, "Bad Gateway");
    }
}