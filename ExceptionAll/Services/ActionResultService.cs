namespace ExceptionAll.Services;

public class ActionResultService : IActionResultService
{
    private readonly IErrorResponseService _errorResponseService;
    private readonly IContextConfigurationService _configurationService;
    private ILogger<IActionResultService> Logger { get; }

    public ActionResultService(
        ILogger<IActionResultService> logger,
        IErrorResponseService errorResponseService,
        IContextConfigurationService configurationService)
    {
        Logger                = logger;
        _errorResponseService = errorResponseService;
        _configurationService = configurationService;
    }

    public IActionResult GetErrorResponse(ExceptionContext context)
    {
        var errorResponse = _errorResponseService.GetErrorResponse(context.Exception);

        if (errorResponse is null)
        {
            Logger.LogInformation(
                context.Exception,
                "Exception type not found when accessing error response container. Please verify you have added this given exception type to your configuration: {type}",
                context.Exception.GetType()
                       .FullName);
        }

        var apiResponse = new ApiErrorDetails
        {
            Title          = errorResponse?.ErrorTitle ?? "ExceptionAll Error",
            StatusCode     = errorResponse?.StatusCode ?? 500,
            Message        = errorResponse?.Message    ?? "There was an error encountered. If no errors are explicitly shown, please see logs for more details.",
            ContextDetails = _configurationService.GetContextDetails(context.HttpContext)
        };

        errorResponse?.LogAction?.Invoke(Logger, context.Exception);
        context.HttpContext.Response.StatusCode = apiResponse.StatusCode;

        return new ObjectResult(apiResponse)
        {
            StatusCode = apiResponse.StatusCode
        };
    }

    public IActionResult GetResponse<T>(ActionContext context, string? message = null, List<ErrorDetail>? errors = null)
        where T : IDetailBuilder, new()
    {
        var (statusCode, title) = new T().GetDetails();

        var apiResponse = new ApiErrorDetails
        {
            Title          = title,
            StatusCode     = statusCode,
            Message        = message ?? "There was an error encountered",
            ContextDetails = _configurationService.GetContextDetails(context.HttpContext, errors)
        };

        context.HttpContext.Response.StatusCode = apiResponse.StatusCode;

        return new ObjectResult(apiResponse)
        {
            StatusCode = apiResponse.StatusCode
        };
    }
}