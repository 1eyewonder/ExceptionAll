using ExceptionAll.Models;

namespace ExceptionAll.Examples;

public class BadGatewayDetailsExample : IExamplesProvider<BadGatewayDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public BadGatewayDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService ?? throw new ArgumentNullException(nameof(contextConfigurationService));
    }

    public BadGatewayDetails GetExamples()
    {
        return new BadGatewayDetails()
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