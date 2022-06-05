namespace ExceptionAll;

/// <summary>
/// Catches exceptions and creates standard responses
/// </summary>
public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly IActionResultService _actionResultService;

    public ExceptionFilter(IActionResultService actionResultService)
    {
        _actionResultService = actionResultService;
    }

    public override void OnException(ExceptionContext context)
    {
        context.Result = _actionResultService.GetErrorResponse(context);
    }
}
