namespace ExceptionAll.Details;

public class InternalServerErrorDetails : BaseDetails
{
    public InternalServerErrorDetails(ActionContext context, string? title = null, string? message = null, List<ErrorDetail>? errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Internal Server Error" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status500InternalServerError,
        string.IsNullOrEmpty(message) ? "See errors or logs for more details" : message,
        errors ?? new List<ErrorDetail>())
    {
    }
    public InternalServerErrorDetails(ExceptionContext context, string? title = null, string? message = null, List<ErrorDetail>? errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Internal Server Error" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status500InternalServerError,
        string.IsNullOrEmpty(message) ? "See errors or logs for more details" : message,
        errors ?? new List<ErrorDetail>())
    {
    }
}
