namespace ExceptionAll;

/// <inheritdoc cref="IErrorResponseService" />
public class ErrorResponseService : IErrorResponseService
{
    private readonly ILogger<ErrorResponseService> _logger;
    private Dictionary<Type, IErrorResponse> ErrorResponses { get; } = new();

    public ErrorResponseService(ILogger<ErrorResponseService> logger, IContextConfigurationService configurationService)
    {
        _logger = logger;
        AddErrorResponses(configurationService.GetConfiguration().ErrorResponses);
    }

    /// <inheritdoc cref="IErrorResponseService.GetErrorResponse{T}" />
    public IErrorResponse? GetErrorResponse<T>(T exception) where T : Exception
    {
        ErrorResponses.TryGetValue(exception.GetType(), out var errorResponse);

        if (errorResponse is null)
            _logger.LogInformation(
                exception,
                "Exception type not found when accessing error response container. Please verify you have added this given exception type to your configuration: {Type}",
                exception.GetType().FullName
            );

        return errorResponse;
    }

    /// <summary>
    ///     Creates the collection of configured responses by the user
    /// </summary>
    /// <param name="errorResponses"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private void AddErrorResponses(ICollection<IErrorResponse> errorResponses)
    {
        if (!errorResponses.Any())
            throw new ArgumentNullException(nameof(errorResponses));

        foreach (var response in errorResponses)
        {
            if (ErrorResponses.ContainsKey(response.ExceptionType))
            {
                _logger.LogError(
                    "Cannot add response to ErrorResponseService because an error response already exists for the exception type: {Type}",
                    response.ExceptionType
                );

                throw new ArgumentException(
                    $"Exception type, {response.ExceptionType}, already exists in service collection"
                );
            }

            ErrorResponses.Add(response.ExceptionType, response);
        }
    }
}