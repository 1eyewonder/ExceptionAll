namespace ExceptionAll.Examples;

public class NotFoundDetailsExample : IExamplesProvider<NotFoundDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public NotFoundDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public NotFoundDetails GetExamples()
    {
        return new NotFoundDetails()
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