namespace ExceptionAll.Details;

public class BaseDetails : ProblemDetails
{
    public BaseDetails(ActionContext context, string title, string instance, int? status, string details, List<ErrorDetail> errors)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        Title = title;
        Instance = instance;
        Status = status;
        Detail = details;
        this.AddDefaultExtensionsFromContext(context, errors);
    }

    public BaseDetails(ExceptionContext context, string title, string instance, int? status, string details, List<ErrorDetail> errors)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        Title = title;
        Instance = instance;
        Status = status;
        Detail = details;
        this.AddDefaultExtensionsFromContext(context, errors);
    }
}
