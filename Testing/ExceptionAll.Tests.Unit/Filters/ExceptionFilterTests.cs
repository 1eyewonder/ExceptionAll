using ExceptionAll.Filters;

namespace ExceptionAll.Tests.Unit.Filters;

public class ExceptionFilterTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ExceptionFilterTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void NewExceptionFilter_OnConstruction_SuccessfullyInitializes()
    {
        // Arrange
        var mockActionResultService = Substitute.For<IActionResultService>();

        // Act
        var exceptionFilter = new ExceptionFilter(mockActionResultService);

        // Assess
        _testOutputHelper.WriteLine("Expected exception filter object");
        _testOutputHelper.WriteLine(exceptionFilter.ToJson());

        // Assert
        Assert.NotNull(exceptionFilter);

    }

    [Fact]
    public void OnException_WithExceptionContext_SuccessfullyRuns()
    {
        // Arrange
        var mockActionResultService = Substitute.For<IActionResultService>();
        var exceptionFilter         = new ExceptionFilter(mockActionResultService);
        var mockExceptionContext    = TestHelper.GetMockExceptionContext<Exception>();

        // Act
        var action    = new Action(() => exceptionFilter.OnException(mockExceptionContext.Object));
        var exception = Record.Exception(action);

        // Assess
        _testOutputHelper.WriteLine("Expected exception: null");
        _testOutputHelper.WriteLine($"Actual exception: {exception?.Message ?? null}");

        // Act
        Assert.Null(exception);
    }
}