namespace ExceptionAll.Examples;

public class InternalServerErrorDetailsExample : IExamplesProvider<InternalServerErrorDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public InternalServerErrorDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public InternalServerErrorDetails GetExamples()
    {
        return new InternalServerErrorDetails()
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