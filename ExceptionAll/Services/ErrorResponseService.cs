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

    private void AddErrorResponses(List<IErrorResponse> errorResponses)
    {
        if (errorResponses == null || !errorResponses.Any())
            throw new ArgumentNullException(nameof(errorResponses));

        foreach (var errorResponse in errorResponses)
            AddErrorResponse(errorResponse);
    }

    public void AddErrorResponse(IErrorResponse response)
    {
        new ErrorResponseValidator().ValidateAndThrow(response);

        if (ErrorResponses.ContainsKey(response.ExceptionType))
        {
            _logger.LogError(
                "Cannot add response to service because an error response already exists for the exception type: {0}",
                response.ExceptionType);

            throw new ArgumentException(
                $"Exception type, {response.ExceptionType}, already exists in service collection");
        }

        ErrorResponses.Add(response.ExceptionType, response);
    }

    public Dictionary<Type, IErrorResponse> GetErrorResponses()
    {
        return ErrorResponses;
    }
}