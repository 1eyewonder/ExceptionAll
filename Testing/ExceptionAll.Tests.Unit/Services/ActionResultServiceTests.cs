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
        var apiErrorDetailsService = Substitute.For<IApiErrorDetailsService>();
        var contextConfigurationService = Substitute.For<IContextConfigurationService>();
        const string message = "This is a test message";
        var exception = new Exception(message);


        apiErrorDetailsService.GetErrorDetails(Arg.Any<HttpContext>(), exception)
                              .Returns(
                                  new ApiErrorDetails { Message = message, Title = "Testing123", StatusCode = 500 }
                              );

        var exceptionContext = ContextSubstitutes.GetExceptionContext();
        exceptionContext.Exception.Returns(exception);

        var sut = new ActionResultService(apiErrorDetailsService, contextConfigurationService);

        // Act
        var actionResult = sut.GetErrorResponse(exceptionContext) as ObjectResult;

        // Assess
        _testOutputHelper.WriteLine("Expected status code: 500");
        _testOutputHelper.WriteLine($"Actual status code: {actionResult?.StatusCode}");
        _testOutputHelper.WriteLine($"Error details: {actionResult?.Value?.ToJson()}");

        // Assert
        Assert.Equal(500, actionResult?.StatusCode);
    }

    [Fact]
    public void GetResponse_WithValidContext_ReturnsObjectResult()
    {
        // Arrange
        var apiErrorDetailsService = Substitute.For<IApiErrorDetailsService>();
        var contextConfigurationService = Substitute.For<IContextConfigurationService>();
        var actionContext = ContextSubstitutes.GetActionContextSub();

        var sut = new ActionResultService(apiErrorDetailsService, contextConfigurationService);

        // Act
        var actionResult = sut.GetResponse<NotFoundDetails>(actionContext) as ObjectResult;

        // Assess
        _testOutputHelper.WriteLine("Expected status code: 404");
        _testOutputHelper.WriteLine($"Actual status code: {actionResult?.StatusCode}");

        // Assert
        Assert.Equal(404, actionResult?.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetApiErrorDetails))]
    public void GetResponse_ForApiErrorDetails_ReturnsCorrectErrorCode(int errorCode, Type detailType)
    {
        // Arrange
        var apiErrorDetailsService = Substitute.For<IApiErrorDetailsService>();
        var contextConfigurationService = Substitute.For<IContextConfigurationService>();
        var actionContext = ContextSubstitutes.GetActionContextSub();

        var sut = new ActionResultService(apiErrorDetailsService, contextConfigurationService);

        // Act
        var method = typeof(IActionResultService).GetMethod("GetResponse");
        method = method?.MakeGenericMethod(detailType);

       var actionResult = method?.Invoke(sut, new object[] { actionContext, null, null }) as ObjectResult;

        // Assess
        _testOutputHelper.WriteLine($"Expected status code: {errorCode}");
        _testOutputHelper.WriteLine($"Actual status code: {actionResult?.StatusCode}");
        _testOutputHelper.WriteLine($"Detail object type: {detailType.FullName}");

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(errorCode, actionResult?.StatusCode);
    }

    public static IEnumerable<object[]> GetApiErrorDetails()
    {
        return new List<object[]>
        {
            new object[]
            {
                502, typeof(BadGatewayDetails)
            },
            new object[]
            {
                400, typeof(BadRequestDetails)
            },
            new object[]
            {
                409, typeof(ConflictDetails)
            },
            new object[]
            {
                403, typeof(ForbiddenDetails)
            },
            new object[]
            {
                500, typeof(InternalServerErrorDetails)
            },
            new object[]
            {
                404, typeof(NotFoundDetails)
            },
            new object[]
            {
                501, typeof(NotImplementedDetails)
            },
            new object[]
            {
                408, typeof(RequestTimeoutDetails)
            },
            new object[]
            {
                503, typeof(ServiceUnavailableDetails)
            },
            new object[]
            {
                429, typeof(TooManyRequestsDetails)
            },
            new object[]
            {
                401, typeof(UnauthorizedDetails)
            },
            new object[]
            {
                415, typeof(UnsupportedMediaTypeDetails)
            }
        };
    }
}