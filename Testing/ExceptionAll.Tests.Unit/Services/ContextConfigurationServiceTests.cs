namespace ExceptionAll.Tests.Unit.Services;

public class ContextConfigurationServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ContextConfigurationServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetContextDetails_WithNullErrors_ShouldReturnDictionaryWithoutErrorsKey()
    {
        // Arrange
        var configuration = Substitute.For<IExceptionAllConfiguration>();
        configuration.ContextConfiguration.Returns(new Dictionary<string, Func<HttpContext, object>>());

        var sut = new ContextConfigurationService(configuration);

        // Act
        var dictionary   = sut.GetContextDetails(new DefaultHttpContext());
        var hasErrorsKey = dictionary!.TryGetValue("Errors", out var value);

        // Assess
        _testOutputHelper.WriteLine($"Expected type: {typeof(IReadOnlyDictionary<string, object>)}");

        //Assert
        Assert.IsType<Dictionary<string, object>>(dictionary);
        Assert.False(hasErrorsKey);
    }

    [Fact]
    public void GetContextDetails_WithEmptyErrors_ShouldReturnDictionaryWithoutErrorsKey()
    {
        // Arrange
        var configuration = Substitute.For<IExceptionAllConfiguration>();
        configuration.ContextConfiguration.Returns(new Dictionary<string, Func<HttpContext, object>>());

        var sut = new ContextConfigurationService(configuration);

        // Act
        var dictionary   = sut.GetContextDetails(new DefaultHttpContext(), new List<ErrorDetail>());
        var hasErrorsKey = dictionary!.TryGetValue("Errors", out var value);

        // Assess
        _testOutputHelper.WriteLine($"Expected type: {typeof(IReadOnlyDictionary<string, object>)}");

        //Assert
        Assert.IsType<Dictionary<string, object>>(dictionary);
        Assert.False(hasErrorsKey);
    }

    [Fact]
    public void GetContextDetails_WithErrors_ShouldReturnDictionaryWithErrorsKey()
    {
        // Arrange
        var configuration = Substitute.For<IExceptionAllConfiguration>();
        configuration.ContextConfiguration.Returns(new Dictionary<string, Func<HttpContext, object>>());

        var sut = new ContextConfigurationService(configuration);

        // Act
        var dictionary = sut.GetContextDetails(
            new DefaultHttpContext(),
            new List<ErrorDetail>() { new ("Error", "Message") });

        var hasErrorsKey = dictionary!.TryGetValue("Errors", out var value);

        // Assess
        _testOutputHelper.WriteLine($"Expected type: {typeof(IReadOnlyDictionary<string, object>)}");

        //Assert
        Assert.IsType<Dictionary<string, object>>(dictionary);
        Assert.True(hasErrorsKey);
    }

    [Fact]
    public void GetContextDetails_WithErrorsAndNullContextMapping_ShouldReturnDictionaryWithErrorsKey()
    {
        // Arrange
        var configuration = Substitute.For<IExceptionAllConfiguration>();
        var sut           = new ContextConfigurationService(configuration);

        // Act
        var dictionary = sut.GetContextDetails(
            new DefaultHttpContext(),
            new List<ErrorDetail>() { new ("Error", "Message") });

        var hasErrorsKey = dictionary!.TryGetValue("Errors", out var value);

        // Assess
        _testOutputHelper.WriteLine($"Expected type: {typeof(IReadOnlyDictionary<string, object>)}");

        //Assert
        Assert.IsType<Dictionary<string, object>>(dictionary);
        Assert.True(hasErrorsKey);
    }

    [Fact]
    public void GetConfiguration_ReturnsUtilizedConfiguration()
    {
        // Arrange
        var dictionary = new Dictionary<string, Func<HttpContext, object>> { { "Test", x => x.TraceIdentifier } };
        var errorResponses = new List<IErrorResponse>
        {
            ErrorResponse
                .CreateErrorResponse()
                .WithTitle("Argument Null Exception")
                .WithStatusCode(500)
                .WithMessage("The developer goofed")
                .ForException<ArgumentNullException>()
                .WithLogAction((x, e) => x.LogDebug(e, "Oops I did it again"))
        };
        
        var configuration = Substitute.For<IExceptionAllConfiguration>();
        configuration.ContextConfiguration.Returns(dictionary);
        configuration.ErrorResponses.Returns(errorResponses);

        var sut = new ContextConfigurationService(configuration);
        
        // Act
        var returnedConfiguration = sut.GetConfiguration();

        // Assess
        _testOutputHelper.WriteLine($"Expected configuration: {configuration}");
        _testOutputHelper.WriteLine($"Actual configuration: {returnedConfiguration}");

        // Assert
        Assert.Equal(dictionary, returnedConfiguration.ContextConfiguration);
        Assert.Equal(errorResponses, returnedConfiguration.ErrorResponses);
    }
}