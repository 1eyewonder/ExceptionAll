using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Interfaces;
using ExceptionAll.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;
using Xunit.Abstractions;

namespace ExceptionAll.Tests.Services
{
    public class ActionResultServiceTests : TestBase
    {
        public ActionResultServiceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void GetResponse_WithOutOfBoxDetails_ReturnsObjectResult()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();
            var mockErrorResponseService = TestHelper.GetMockErrorResponseService();
            var mockActionLogger = new Mock<ILogger<IActionResultService>>();
            var actionResultService = new ActionResultService(mockActionLogger.Object,
                mockErrorResponseService.Object);

            // Act
            var result = actionResultService.GetResponse<InternalServerErrorDetails>(mockActionContext.Object);

            // Assess
            TestOutputHelper.WriteLine($"Error Responses: {result.ToJson()}");

            // Assert
            Assert.NotNull(((ObjectResult)result).Value);
            Assert.IsType<InternalServerErrorDetails>(((ObjectResult)result).Value);
        }

        [Fact]
        public void GetResponse_WithProblemDetails_Throws()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();
            var mockErrorResponseService = TestHelper.GetMockErrorResponseService();
            var mockActionLogger = new Mock<ILogger<IActionResultService>>();
            var actionResultService = new ActionResultService(mockActionLogger.Object,
                mockErrorResponseService.Object);

            // Act
            var action = new Func<IActionResult>(() => actionResultService.GetResponse<ProblemDetails>(mockActionContext.Object));

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void GetResponse_WithInvalidInheritedConstructor_Throws()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();
            var mockErrorResponseService = TestHelper.GetMockErrorResponseService();
            var mockActionLogger = new Mock<ILogger<IActionResultService>>();
            var actionResultService = new ActionResultService(mockActionLogger.Object,
                mockErrorResponseService.Object);

            // Act
            var action = new Func<IActionResult>(() => actionResultService.GetResponse<TestDummy>(mockActionContext.Object));

            // Assert
            Assert.Throws<Exception>(action);
        }

        private class TestDummy : ProblemDetails
        {
        }

        [Fact]
        public void GetErrorResponse_WithValidExceptionType_ReturnsObjectResult()
        {
            // Arrange
            var mockExceptionContext = TestHelper.GetMockExceptionContext<Exception>();
            var mockErrorResponseService = TestHelper.GetMockErrorResponseService();
            var mockActionLogger = new Mock<ILogger<IActionResultService>>();
            var actionResultService = new ActionResultService(mockActionLogger.Object,
                mockErrorResponseService.Object);

            mockErrorResponseService.Object.AddErrorResponse(
                ErrorResponse
                    .CreateErrorResponse()
                    .WithTitle("Test Title")
                    .ForException<Exception>()
                    .WithReturnType<InternalServerErrorDetails>()
                    .WithLogAction((x, e) => x.LogError(e, "Error"))
            );

            // Act
            var result = actionResultService.GetErrorResponse(mockExceptionContext.Object);

            // Assess
            TestOutputHelper.WriteLine($"Error Responses: {result.ToJson()}");

            // Assert
            Assert.NotNull(((ObjectResult)result).Value);
            Assert.IsType<InternalServerErrorDetails>(((ObjectResult)result).Value);
        }
    }
}
