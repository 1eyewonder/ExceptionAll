namespace ExceptionAll.Abstractions;

public class NotImplementedDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (501, "Not Implemented");
    }
}