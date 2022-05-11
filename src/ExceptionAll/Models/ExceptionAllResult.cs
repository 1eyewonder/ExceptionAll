namespace ExceptionAll;

/// <summary>
/// Custom implementation of IResult
/// </summary>
public class ExceptionAllResult : IResult
{
    private readonly ApiErrorDetails _details;

    public ExceptionAllResult(ApiErrorDetails details)
    {
        _details = details;
    }
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        var jsonDetails = JsonSerializer.Serialize(_details);
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(jsonDetails);
        httpContext.Response.StatusCode = _details.StatusCode;
        await httpContext.Response.WriteAsync(jsonDetails);
    }
}