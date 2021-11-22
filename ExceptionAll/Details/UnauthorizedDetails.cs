namespace ExceptionAll.Details;

public class UnauthorizedDetails : BaseDetails
{
    public UnauthorizedDetails(ActionContext context, string title = null, string message = null, List<ErrorDetail> errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Unauthorized" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status401Unauthorized,
        string.IsNullOrEmpty(message) ? "See errors or logs for more details" : message,
        errors)
    {
    }
    public UnauthorizedDetails(ExceptionContext context, string title = null, string message = null, List<ErrorDetail> errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Unauthorized" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status401Unauthorized,
        string.IsNullOrEmpty(message) ? "See errors or logs for more details" : message,
        errors)
    {
    }
}
