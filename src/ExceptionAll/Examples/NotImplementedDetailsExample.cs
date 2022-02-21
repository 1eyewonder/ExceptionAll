namespace ExceptionAll.Examples;

public class NotImplementedDetailsExample : IExamplesProvider<NotImplementedDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public NotImplementedDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public NotImplementedDetails GetExamples()
    {
        return new NotImplementedDetails()
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