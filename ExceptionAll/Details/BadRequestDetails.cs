namespace ExceptionAll.Details;

public class BadRequestDetails : BaseDetails
{
    public BadRequestDetails(ActionContext context, string title = null, string message = null, List<ErrorDetail> errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Bad Request" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status400BadRequest,
        string.IsNullOrEmpty (message) ? "See errors or logs for more details" : message,
        errors)
    { 
    }
    public BadRequestDetails(ExceptionContext context, string title = null, string message = null, List<ErrorDetail> errors = null) :
        base(
        context ?? throw new ArgumentNullException(nameof(context)),
        string.IsNullOrEmpty(title) ? "Bad Request" : title,
        context.HttpContext.Request.Path,
        StatusCodes.Status400BadRequest,
        string.IsNullOrEmpty(message) ? "See errors or logs for more details" : message,
        errors)
    {
    }
}
