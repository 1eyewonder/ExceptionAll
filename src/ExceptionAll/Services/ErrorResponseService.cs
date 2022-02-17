namespace ExceptionAll.Services;

public class ErrorResponseService : IErrorResponseService
{
    private readonly ILogger<IErrorResponseService> _logger;
    private Dictionary<Type, IErrorResponse> ErrorResponses { get; } = new();

    public ErrorResponseService(ILogger<IErrorResponseService> logger, IExceptionAllConfiguration configuration)
    {
        _logger = logger;
        AddErrorResponses(configuration.ErrorResponses);
    }

    public IErrorResponse? GetErrorResponse<T>(T exception) where T : Exception
    {
        ErrorResponses.TryGetValue(exception.GetType(), out var errorResponse);
        return errorResponse;
    }

    private void AddErrorResponses(List<IErrorResponse> errorResponses)
    {
        if (!errorResponses.Any())
            throw new ArgumentNullException(nameof(errorResponses));

        foreach (var response in errorResponses)
        {
            if (ErrorResponses.ContainsKey(response.ExceptionType))
            {
                _logger.LogError(
                    "Cannot add response to ErrorResponseService because an error response already exists for the exception type: {type}",
                    response.ExceptionType);

                throw new ArgumentException(
                    $"Exception type, {response.ExceptionType}, already exists in service collection");
            }

            ErrorResponses.Add(response.ExceptionType, response);
        }
    }
}