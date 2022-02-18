namespace ExceptionAll.Examples;

public class UnsupportedMediaTypeDetailsExample : IExamplesProvider<UnsupportedMediaTypeDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public UnsupportedMediaTypeDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public UnsupportedMediaTypeDetails GetExamples()
    {
        return new UnsupportedMediaTypeDetails()
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