namespace ExceptionAll.Abstractions;

public class NotFoundDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (404, "Not Found");
    }
}