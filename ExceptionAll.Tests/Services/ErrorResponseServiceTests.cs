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
            var mockLogger = new Mock<ILogger<IErrorResponseService>>();
            var mockService = new Mock<ErrorResponseService>(mockLogger.Object);
            var responses = new List<ErrorResponse>
            {
                new()
                {
                    ExceptionType = typeof(OperationCanceledException),
                    DetailsType = typeof(NotFoundDetails)
                },

                new()
                {
                    ExceptionType = typeof(ValidationException),
                    DetailsType = typeof(NotFoundDetails)
                },
            };

            // Act
            mockService.Object.AddErrorResponses(responses);

            // Assess
            TestOutputHelper.WriteLine($"Error Responses: {responses.ToJson()}");

            // Assert
            Assert.NotNull(mockService.Object.GetErrorResponses());
            Assert.NotEmpty(mockService.Object.GetErrorResponses());
        }

        [Fact]
        public void AddErrorResponses_ShouldThrow()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<IErrorResponseService>>();
            var mockService = new Mock<ErrorResponseService>(mockLogger.Object);
            var responses = new List<ErrorResponse>
            {
                new()
                {
                    ExceptionType = typeof(OperationCanceledException),
                    DetailsType = typeof(NotFoundDetails)
                },

                new()
                {
                    ExceptionType = typeof(OperationCanceledException),
                    DetailsType = typeof(NotFoundDetails)
                },
            };

            // Act
            var action = new Action(() => mockService.Object.AddErrorResponses(responses));

            // Assess
            TestOutputHelper.WriteLine($"Error Responses: {responses.ToJson()}");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        public static IEnumerable<object[]> GetValidErrorResponses()
        {
            return TestHelper.GetValidErrorResponses().ToList();
        }
    }
}