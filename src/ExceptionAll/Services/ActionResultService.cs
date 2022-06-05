namespace ExceptionAll;

/// <inheritdoc cref="IActionResultService" />
public class ActionResultService : IActionResultService
{
    private readonly IApiErrorDetailsService _apiErrorDetailsService;
    private readonly IContextConfigurationService _configurationService;

    public ActionResultService(IApiErrorDetailsService apiErrorDetailsService,
                               IContextConfigurationService configurationService)
    {
        _apiErrorDetailsService = apiErrorDetailsService;
        _configurationService   = configurationService;
    }

    /// <exception cref="Exception" />
    /// <inheritdoc cref="IActionResultService.GetErrorResponse" />
    public IActionResult GetErrorResponse(ExceptionContext context)
    {
        var apiResponse = _apiErrorDetailsService.GetErrorDetails(context.HttpContext, context.Exception);
        context.HttpContext.Response.StatusCode = apiResponse.StatusCode;

        return new ObjectResult(apiResponse)
        {
            StatusCode = apiResponse.StatusCode
        };
    }

    /// <inheritdoc cref="IActionResultService.GetResponse{T}" />
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
            StatusCode = apiResponse.StatusCode,
            Value      = apiResponse
        };
    }
}