namespace ExceptionAll.Tests.Unit.Services;

public class ErrorResponseServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ErrorResponseServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [MemberData(nameof(GetValidErrorResponses))]
    public void AddErrorResponse_ShouldSuccessfullyAdd_WhenResponseDoesNotExistInContainerYet(ErrorResponse response)
    {
        // Arrange
        var errorResponseLogger = Substitute.For<ILogger<ErrorResponseService>>();
        var configuration = Substitute.For<IExceptionAllConfiguration>();
        configuration.ErrorResponses.Returns(new List<IErrorResponse> { response });

        var configurationService = Substitute.For<IContextConfigurationService>();
        configurationService.GetConfiguration().Returns(configuration);

        // Act
        var exception = Record.Exception(() => new ErrorResponseService(errorResponseLogger, configurationService));

        // Assess
        _testOutputHelper.WriteLine($"Error Response: {response.ToJson()}");
        _testOutputHelper.WriteLine($"No exception expected. Exception message: {exception?.Message}");

        // Act
        Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(GetValidErrorResponses))]
    public void AddErrorResponse_WithDuplicateResponses_ShouldThrow(ErrorResponse response)
    {
        // Arrange
        var errorResponseLogger = Substitute.For<ILogger<ErrorResponseService>>();
        var configuration = Substitute.For<IExceptionAllConfiguration>();
        configuration.ErrorResponses.Returns(new List<IErrorResponse> { response, response });

        var configurationService = Substitute.For<IContextConfigurationService>();
        configurationService.GetConfiguration().Returns(configuration);

        // Act
        var exception = Record.Exception(() => new ErrorResponseService(errorResponseLogger, configurationService));

        // Assess
        _testOutputHelper.WriteLine($"Error Response: {response.ToJson()}");
        _testOutputHelper.WriteLine($"Exception expected. Exception message: {exception?.Message}");

        // Act
        Assert.NotNull(exception);
    }

    public static IEnumerable<object[]> GetValidErrorResponses()
    {
        return new List<object[]>
        {
            new object[]
            {
                ErrorResponse.CreateErrorResponse()
                             .WithTitle("Test")
                             .WithStatusCode(400)
                             .WithMessage("Model error")
                             .ForException<ValidationException>()
                             .WithLogAction((x, e) => x.LogInformation(e, "There was a model error"))
            },
            new object[]
            {
                ErrorResponse.CreateErrorResponse()
                             .WithTitle("Test")
                             .WithStatusCode(400)
                             .WithMessage("Model error")
                             .ForException<ValidationException>()
            },
            new object[]
            {
                ErrorResponse.CreateErrorResponse()
                             .WithTitle("Test")
                             .WithStatusCode(400)
                             .WithMessage("Model error")
            },
            new object[]
            {
                ErrorResponse.CreateErrorResponse()
                             .WithTitle("Test")
                             .WithStatusCode(400)
            },
            new object[]
            {
                ErrorResponse.CreateErrorResponse()
                             .WithTitle("Test")
            },
            new object[]
            {
                ErrorResponse.CreateErrorResponse()
            }
        };
    }
}