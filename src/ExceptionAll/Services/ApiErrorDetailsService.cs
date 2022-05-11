namespace ExceptionAll;

public class ApiErrorDetailsService : IApiErrorDetailsService
{
    private readonly ILogger<ApiErrorDetailsService> _logger;
    private readonly IErrorResponseService _errorResponseService;
    private readonly IContextConfigurationService _contextConfigurationService;

    public ApiErrorDetailsService(ILogger<ApiErrorDetailsService> logger,
                                  IErrorResponseService errorResponseService,
                                  IContextConfigurationService contextConfigurationService)
    {
        _logger                      = logger;
        _errorResponseService        = errorResponseService;
        _contextConfigurationService = contextConfigurationService;
    }

    /// <inheritdoc cref="IApiErrorDetailsService.GetErrorDetails" />
    public ApiErrorDetails GetErrorDetails(HttpContext context, Exception exception)
    {
        var errorResponse = _errorResponseService.GetErrorResponse(exception);
        errorResponse?.LogAction?.Invoke(_logger, exception);

        return new ApiErrorDetails
        {
            Title      = errorResponse?.ErrorTitle ?? "ExceptionAll Error",
            StatusCode = errorResponse?.StatusCode ?? 500,
            Message = errorResponse?.Message ??
                      "There was an error encountered. If no errors are explicitly shown, please see logs for more details.",
            ContextDetails = _contextConfigurationService.GetContextDetails(context)
        };
    }
}