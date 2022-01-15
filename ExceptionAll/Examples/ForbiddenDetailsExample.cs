using ExceptionAll.Models;

namespace ExceptionAll.Examples;

public class ForbiddenDetailsExample : IExamplesProvider<ForbiddenDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public ForbiddenDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService ?? throw new ArgumentNullException(nameof(contextConfigurationService));
    }
    public ForbiddenDetails GetExamples()
    {
        return new  ForbiddenDetails()
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