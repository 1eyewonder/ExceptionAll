namespace ExceptionAll.Tests.Unit.Services;

public class ErrorResponseServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ErrorResponseServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory, MemberData(nameof(GetValidErrorResponses))]
    public void AddErrorResponse_ShouldSuccessfullyAdd_WhenResponseDoesNotExistInContainerYet(ErrorResponse response)
    {
        // Arrange
        var errorResponseLogger = Substitute.For<ILogger<IErrorResponseService>>();
        var sut = new ErrorResponseService(errorResponseLogger);

        // Act
        sut.AddErrorResponse(response);

        // Assess
        _testOutputHelper.WriteLine($"Error Response: {response.ToJson()}");

        // Act
        Assert.NotNull(sut.GetErrorResponses());
        Assert.NotEmpty(sut.GetErrorResponses());
    }

    [Fact]
    public void AddErrorResponse_WithExistingResponse_ShouldThrow()
    {
        // Arrange
        var errorResponseLogger = Substitute.For<ILogger<IErrorResponseService>>();
        var sut                 = new ErrorResponseService(errorResponseLogger);

        var response = ErrorResponse.CreateErrorResponse()
                                    .WithTitle("Test")
                                    .WithStatusCode(400)
                                    .WithMessage("Model error")
                                    .ForException<ValidationException>()
                                    .WithLogAction((x, e) => x.LogInformation(e, "There was a model error"));
        sut.AddErrorResponse(response);

        // Act
        var exception = Record.Exception(() => sut.AddErrorResponse(response));

        // Assess
        _testOutputHelper.WriteLine($"Error Response: {response.ToJson()}");
        _testOutputHelper.WriteLine($"Exception: {exception.Message}");

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
            },
        };
    }
}