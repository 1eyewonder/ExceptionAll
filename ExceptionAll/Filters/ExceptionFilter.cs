namespace ExceptionAll.Filters;

/// <summary>
/// Catches exceptions and creates standard responses using the IActionResultService
/// </summary>
public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly IActionResultService _actionResultService;

    public ExceptionFilter(IActionResultService actionResultService)
    {
        _actionResultService = actionResultService ?? throw new ArgumentNullException(nameof(actionResultService));
    }

    public override void OnException(ExceptionContext context)
    {
        context.Result = _actionResultService.GetErrorResponse(context);
        return;
        /*if (context.ModelState.IsValid)
        {
            
        }

        var errors = new List<ErrorDetail>();
        foreach (var (key, value) in context.ModelState)
        {
            errors.AddRange(value.Errors.Select(error => new ErrorDetail(key, error.ErrorMessage)));
        }

        context.Result = _actionResultService.GetResponse<BadRequestDetails>(
            context,
            "Invalid request model",
            errors.Any() ? errors : null);*/
    }
}
