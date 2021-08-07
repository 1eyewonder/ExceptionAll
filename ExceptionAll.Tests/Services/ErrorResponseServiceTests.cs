using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Services;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ExceptionAll.Interfaces;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace ExceptionAll.Tests.Services
{
    public class ErrorResponseServiceTests : TestBase
    {
        public ErrorResponseServiceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory, MemberData(nameof(GetValidErrorResponses))]
        public void AddErrorResponse_ShouldSuccessfullyAdd(ErrorResponse response)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<IErrorResponseService>>();
            var mockService = new Mock<ErrorResponseService>(mockLogger.Object);

            // Act
            mockService.Object.AddErrorResponse(response);

            // Assess
            TestOutputHelper.WriteLine($"Error Response: {response.ToJson()}");

            // Act
            Assert.NotNull(mockService.Object.GetErrorResponses());
            Assert.NotEmpty(mockService.Object.GetErrorResponses());
        }

        [Fact]
        public void AddErrorResponses_ShouldSuccessfullyAdd()
        {
            // Arrange
            var mockErsLogger = new Mock<ILogger<IErrorResponseService>>();
            var mockErrorResponseService = new Mock<ErrorResponseService>(mockErsLogger.Object);
            var mockActionResultService = TestHelper.GetMockActionResultService();

            var responses = new List<ErrorResponse>
            {
                ErrorResponse
                    .CreateErrorResponse(mockActionResultService.Object)
                    .ForException(typeof(OperationCanceledException))
                    .WithReturnType(typeof(NotFoundDetails))
                    .WithLogAction(null),

                ErrorResponse
                    .CreateErrorResponse(mockActionResultService.Object)
                    .ForException(typeof(ValidationException))
                    .WithReturnType(typeof(NotFoundDetails))
                    .WithLogAction(null)
            };

            // Act
            foreach (var response in responses)
            {
                mockErrorResponseService.Object.AddErrorResponse(response);
            }

            // Assess
            TestOutputHelper.WriteLine($"Error Responses: {responses.ToJson()}");

            // Assert
            Assert.NotNull(mockErrorResponseService.Object.GetErrorResponses());
            Assert.NotEmpty(mockErrorResponseService.Object.GetErrorResponses());
        }

        [Fact]
        public void AddErrorResponse_ShouldThrow()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<IErrorResponseService>>();
            var mockService = new Mock<ErrorResponseService>(mockLogger.Object);
            var mockActionResultService = TestHelper.GetMockActionResultService();
            var response = ErrorResponse
                .CreateErrorResponse(mockActionResultService.Object)
                .ForException(typeof(OperationCanceledException))
                .WithReturnType(typeof(NotFoundDetails));

            // Act
            mockService.Object.AddErrorResponse(response);
            var action = new Action(() => mockService.Object.AddErrorResponse(response));

            // Assess
            TestOutputHelper.WriteLine($"Error Responses: {response.ToJson()}");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        public static IEnumerable<object[]> GetValidErrorResponses()
        {
            var mockActionResultService = TestHelper.GetMockActionResultService();
            return TestHelper.GetValidErrorResponses(mockActionResultService.Object).ToList();
        }
    }
}