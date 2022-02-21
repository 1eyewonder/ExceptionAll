namespace ExceptionAll.Examples;

public class ConflictDetailsExample : IExamplesProvider<ConflictDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public ConflictDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public ConflictDetails GetExamples()
    {
        return new ConflictDetails()
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