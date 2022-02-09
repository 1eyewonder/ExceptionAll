namespace ExceptionAll.Examples;

public class UnauthorizedDetailsExample : IExamplesProvider<UnauthorizedDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public UnauthorizedDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public UnauthorizedDetails GetExamples()
    {
        return new UnauthorizedDetails()
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