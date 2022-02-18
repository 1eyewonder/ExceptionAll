namespace ExceptionAll.Abstractions.Details;

public class RequestTimeoutDetailsExample : IExamplesProvider<RequestTimeoutDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public RequestTimeoutDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public RequestTimeoutDetails GetExamples()
    {
        return new RequestTimeoutDetails()
        {
            Message = "Oops, there was an error",
            ContextDetails = _contextConfigurationService.GetContextDetails(
                new DefaultHttpContext(),
                new List<ErrorDetail>
                {
                    new("Error!", "Something broke")
                })
        };
    }
}