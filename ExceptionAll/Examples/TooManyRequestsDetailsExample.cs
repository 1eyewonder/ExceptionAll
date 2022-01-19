namespace ExceptionAll.Examples;

public class TooManyRequestsDetailsExample : IExamplesProvider<TooManyRequestsDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public TooManyRequestsDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public TooManyRequestsDetails GetExamples()
    {
        return new TooManyRequestsDetails()
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