namespace ExceptionAll.Abstractions;

public class UnsupportedMediaTypeDetails : BaseDetails
{
    public override (int StatusCode, string Title) GetDetails()
    {
        return (415, "Unsupported Media Type");
    }
}