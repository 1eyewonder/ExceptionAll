namespace ExceptionAll.Examples;

public class ServiceUnavailableDetailsExample : IExamplesProvider<ServiceUnavailableDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public ServiceUnavailableDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public ServiceUnavailableDetails GetExamples()
    {
        return new ServiceUnavailableDetails()
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