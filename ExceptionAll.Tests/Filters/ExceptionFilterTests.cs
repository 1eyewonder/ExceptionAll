using System;
using ExceptionAll.Filters;
using Xunit;
using Xunit.Abstractions;

namespace ExceptionAll.Tests.Filters
{
    public class ExceptionFilterTests : TestBase
    {

        public ExceptionFilterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void NewExceptionFilter_OnConstruction_SuccessfullyInitializes()
        {
            // Arrange
            var mockActionResultService = TestHelper.GetMockActionResultService();

            // Act
            var exceptionFilter = new ExceptionFilter(mockActionResultService.Object);

            // Assess
            TestOutputHelper.WriteLine(exceptionFilter.ToJson());

            // Assert
            Assert.NotNull(exceptionFilter);

        }

        [Fact]
        public void OnException_WithExceptionContext_SuccessfullyRuns()
        {
            // Arrange
            var mockActionResultService = TestHelper.GetMockActionResultService();
            var exceptionFilter = new ExceptionFilter(mockActionResultService.Object);
            var mockExceptionContext = TestHelper.GetMockExceptionContext<Exception>();

            // Act
            var action = new Action(() => exceptionFilter.OnException(mockExceptionContext.Object));
            var exception = Record.Exception(action);

            // Assess
            TestOutputHelper.WriteLine("Expected exception: null");
            TestOutputHelper.WriteLine($"Actual exception: {exception.ToJson()}");

            // Act
            Assert.Null(exception);

        }
    }
}
