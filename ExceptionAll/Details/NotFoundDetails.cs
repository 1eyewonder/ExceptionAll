namespace ExceptionAll.Details;

public class NotFoundDetails : BaseDetails
{
    public NotFoundDetails(ActionContext context, string? title = null, string? message = null, List<ErrorDetail>? errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Not Found" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status404NotFound,
        string.IsNullOrEmpty(message) ? "See errors or logs for more details" : message,
        errors ?? new List<ErrorDetail>())
    {
    }
    public NotFoundDetails(ExceptionContext context, string? title = null, string? message = null, List<ErrorDetail>? errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Not Found" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status404NotFound,
        string.IsNullOrEmpty(message) ? "See errors or logs for more details" : message,
        errors ?? new List<ErrorDetail>())
    {
    }
}
