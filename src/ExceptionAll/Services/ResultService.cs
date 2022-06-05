namespace ExceptionAll;

/// <inheritdoc cref="IResultService" />
public class ResultService : IResultService
{
    private readonly IApiErrorDetailsService _apiErrorDetailsService;
    private readonly IContextConfigurationService _configurationService;

    public ResultService(IApiErrorDetailsService apiErrorDetailsService,
                         IContextConfigurationService configurationService)
    {
        _apiErrorDetailsService = apiErrorDetailsService;
        _configurationService   = configurationService;
    }

    /// <exception cref="Exception" />
    /// <inheritdoc cref="IResultService.GetErrorResponse" />
    public IResult GetErrorResponse(Exception exception, HttpContext context)
    {
        var apiResponse = _apiErrorDetailsService.GetErrorDetails(context, exception);

        return new ExceptionAllResult(apiResponse);
    }

    /// <inheritdoc cref="IResultService.GetResponse{T}" />
    public IResult GetResponse<T>(HttpContext context, string? message = null, List<ErrorDetail>? errors = null)
        where T : IDetailBuilder, new()
    {
        var (statusCode, title) = new T().GetDetails();
        var apiResponse = new ApiErrorDetails
        {
            Title          = title,
            StatusCode     = statusCode,
            Message        = message ?? "There was an error encountered",
            ContextDetails = _configurationService.GetContextDetails(context, errors)
        };

        return new ExceptionAllResult(apiResponse);
    }
}