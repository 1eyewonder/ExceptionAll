namespace ExceptionAll.Tests.Unit.Services;

public class ActionResultServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ActionResultServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetErrorResponse_WithValidException_ReturnsMatchingResponse()
    {
        // Arrange
        var logger = Substitute.For<ILogger<IActionResultService>>();
        var errorResponseService = Substitute.For<IErrorResponseService>();
        var contextConfigurationService = Substitute.For<IContextConfigurationService>();

        errorResponseService.GetErrorResponse(Arg.Any<Exception>())
                            .Returns(GetErrorResponse());

        var exceptionContext = ContextSubstitutes.GetExceptionContext();
        exceptionContext.Exception.Returns(new Exception("This is a test message"));

        var sut = new ActionResultService(logger, errorResponseService, contextConfigurationService);

        // Act
        var actionResult = sut.GetErrorResponse(exceptionContext) as ObjectResult;

        // Assess
        _testOutputHelper.WriteLine("Expected status code: 400");
        _testOutputHelper.WriteLine($"Actual status code: {actionResult?.StatusCode}");

        // Assert
        Assert.Equal(400, actionResult?.StatusCode);
    }


    [Fact]
    public void GetErrorResponse_WithNullErrorResponse_ReturnsMatchingResponse()
    {
        // Arrange
        var logger = Substitute.For<ILogger<IActionResultService>>();
        var errorResponseService = Substitute.For<IErrorResponseService>();
        var contextConfigurationService = Substitute.For<IContextConfigurationService>();

        errorResponseService.GetErrorResponse(Arg.Any<Exception>())
                            .ReturnsNull();

        var exceptionContext = ContextSubstitutes.GetExceptionContext();
        exceptionContext.Exception.Returns(new Exception("This is a test message"));

        var sut = new ActionResultService(logger, errorResponseService, contextConfigurationService);

        // Act
        var actionResult = sut.GetErrorResponse(exceptionContext) as ObjectResult;

        // Assess
        _testOutputHelper.WriteLine("Expected status code: 500");
        _testOutputHelper.WriteLine($"Actual status code: {actionResult?.StatusCode}");

        // Assert
        Assert.Equal(500, actionResult?.StatusCode);
        logger.ReceivedWithAnyArgs(1).LogInformation(new Exception(), "{Test}", "test");
    }

    [Fact]
    public void GetResponse_WithValidContext_ReturnsObjectResult()
    {
        // Arrange
        var logger = Substitute.For<ILogger<IActionResultService>>();
        var errorResponseService = Substitute.For<IErrorResponseService>();
        var contextConfigurationService = Substitute.For<IContextConfigurationService>();

        errorResponseService.GetErrorResponse(Arg.Any<Exception>())
                            .ReturnsNull();

        var actionContext = ContextSubstitutes.GetActionContextSub();

        var sut = new ActionResultService(logger, errorResponseService, contextConfigurationService);

        // Act
        var actionResult = sut.GetResponse<NotFoundDetails>(actionContext) as ObjectResult;

        // Assess
        _testOutputHelper.WriteLine("Expected status code: 404");
        _testOutputHelper.WriteLine($"Actual status code: {actionResult?.StatusCode}");

        // Assert
        Assert.Equal(404, actionResult?.StatusCode);
    }

    private static ErrorResponse GetErrorResponse()
    {
        return ErrorResponse.CreateErrorResponse()
                            .WithTitle("Test")
                            .WithStatusCode(400)
                            .WithMessage("Model error")
                            .ForException<ValidationException>()
                            .WithLogAction((x, e) => x.LogInformation(e, "There was a model error"));
    }
}