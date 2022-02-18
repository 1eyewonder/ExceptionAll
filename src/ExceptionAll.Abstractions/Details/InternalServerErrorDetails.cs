namespace ExceptionAll.Abstractions.Details;

public class InternalServerErrorDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (500, "Internal Server Error");
    }
}