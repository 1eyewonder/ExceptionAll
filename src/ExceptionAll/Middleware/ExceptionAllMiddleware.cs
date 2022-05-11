namespace ExceptionAll;

public class ExceptionAllMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IResultService _resultService;

    public ExceptionAllMiddleware(RequestDelegate next, IResultService resultService)
    {
        _next = next;
        _resultService = resultService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var result = _resultService.GetErrorResponse(e, context);
            await result.ExecuteAsync(context);
        }
    }
}