using ExceptionAll.Models;

namespace ExceptionAll.Examples;

public class BadRequestDetailsExample : IExamplesProvider<BadRequestDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public BadRequestDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService ?? throw new ArgumentNullException(nameof(contextConfigurationService));
    }

    public BadRequestDetails GetExamples()
    {
        return new BadRequestDetails
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