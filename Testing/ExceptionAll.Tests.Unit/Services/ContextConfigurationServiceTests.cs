using System.Collections.ObjectModel;
using ExceptionAll.Abstractions.Models;

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
}