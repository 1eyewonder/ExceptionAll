namespace ExceptionAll.Details;

public class ForbiddenDetails : BaseDetails
{
    public ForbiddenDetails(ActionContext context, string? title = null, string? message = null, List<ErrorDetail>? errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Forbidden" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status403Forbidden,
        string.IsNullOrEmpty(message) ? "See errors or logs for more details" : message,
        errors ?? new List<ErrorDetail>())
    {
    }
    public ForbiddenDetails(ExceptionContext context, string? title = null, string? message = null, List<ErrorDetail>? errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Forbidden" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status403Forbidden,
        string.IsNullOrEmpty(message) ? "See errors or logs for more details" : message,
        errors ?? new List<ErrorDetail>())
    {
    }
}
