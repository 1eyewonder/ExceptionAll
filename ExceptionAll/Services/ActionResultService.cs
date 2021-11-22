namespace ExceptionAll.Services;

public class ActionResultService : IActionResultService
{
    private readonly IErrorResponseService _errorResponseService;
    private ILogger<IActionResultService> Logger { get; }

    public ActionResultService(ILogger<IActionResultService> logger,
        IErrorResponseService errorResponseService)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _errorResponseService = errorResponseService ?? throw new ArgumentNullException(nameof(errorResponseService));
    }

    public IActionResult GetErrorResponse(ExceptionContext context)
    {
        BaseDetails details;
        if (_errorResponseService.GetErrorResponses()
            .TryGetValue(context.Exception.GetType(),
            out var response))
        {
            new ErrorResponseValidator().ValidateAndThrow(response);
            var constructorInfo = GetExceptionContextConstructor(response.DetailsType);

            details = (BaseDetails)constructorInfo.Invoke(new object[]
            {
                    context, response.ErrorTitle, null, null
            });

            if (details.Status != null)
            {
                context.HttpContext.Response.StatusCode = (int)details.Status;
            }

            response.LogAction?.Invoke(Logger, context.Exception);
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            details = new InternalServerErrorDetails(context, "Internal Server Error");
            Logger.LogError(context.Exception, "Error encountered when accessing resource");
        }

        return new ObjectResult(details)
        {
            StatusCode = context.HttpContext.Response.StatusCode
        };
    }

    public IActionResult GetResponse<T>(ActionContext context, string message = null) where T : BaseDetails
    {
        T details;
        if (!typeof(T).IsSubclassOf(typeof(BaseDetails)) &&
            typeof(T) == typeof(BaseDetails))
        {
            var e = new Exception("ProblemDetails is not an acceptable type");
            Logger.LogError(e, "ProblemDetails is not a valid type for this class. Please refer to documentation for assistance");
            throw e;
        }

        try
        {
            var constructorInfo = ProblemDetailsHelper.GetActionContextConstructor<T>();
            details = (T)constructorInfo.Invoke(new object[] { context, "Caught Exception", message, null });
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw new Exception("Error when trying to invoke object constructor", e);
        }

        new ProblemDetailsValidator<T>().ValidateAndThrow(details);
        if (details.Status != null) context.HttpContext.Response.StatusCode = (int)details.Status;

        Logger.LogTrace(message ?? nameof(T).Replace("Details", "").Trim());
        return new ObjectResult(details)
        {
            StatusCode = details.Status
        };
    }

    private static ConstructorInfo GetExceptionContextConstructor(Type type)
    {
        try
        {
            return type.GetConstructor(new[]
            {
                    typeof(ExceptionContext),
                    typeof(string),
                    typeof(string),
                    typeof(List<ErrorDetail>)
                });
        }
        catch (Exception e)
        {
            throw new Exception($"Error creating constructor for type: {type}", e);
        }
    }
}
