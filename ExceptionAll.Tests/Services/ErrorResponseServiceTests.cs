using ExceptionAll.Details;
using ExceptionAll.Services;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ExceptionAll.Interfaces;
using ExceptionAll.Models;
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
            var service = new ErrorResponseService(mockLogger.Object);

            // Act
            service.AddErrorResponse(response);

            // Assess
            TestOutputHelper.WriteLine($"Error Response: {response.ToJson()}");

            // Act
            Assert.NotNull(service.GetErrorResponses());
            Assert.NotEmpty(service.GetErrorResponses());
        }

        [Fact]
        public void AddErrorResponse_WithInvalidResponse_ShouldThrow()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<IErrorResponseService>>();
            var service = new ErrorResponseService(mockLogger.Object);
            var response = ErrorResponse
                .CreateErrorResponse()
                .WithTitle(null)
                .ForException<Exception>()
                .WithReturnType<InternalServerErrorDetails>()
                .WithLogAction(null);

            // Act
            var action = new Action(() => service.AddErrorResponse(response));

            // Assess
            TestOutputHelper.WriteLine($"Error Response: {response.ToJson()}");

            // Act
            Assert.Throws<ValidationException>(action);
        }

        [Fact]
        public void AddErrorResponses_ShouldSuccessfullyAdd()
        {
            // Arrange
            var mockErsLogger = new Mock<ILogger<IErrorResponseService>>();
            var service = new ErrorResponseService(mockErsLogger.Object);

            var responses = new List<ErrorResponse>
            {
                ErrorResponse
                    .CreateErrorResponse()
                    .ForException<OperationCanceledException>()
                    .WithReturnType<NotFoundDetails>()
                    .WithLogAction(null),

                ErrorResponse
                    .CreateErrorResponse()
                    .ForException<ValidationException>()
                    .WithReturnType<NotFoundDetails>()
                    .WithLogAction(null)
            };

            // Act
            foreach (var response in responses)
            {
                service.AddErrorResponse(response);
            }

            // Assess
            TestOutputHelper.WriteLine($"Error Responses: {responses.ToJson()}");

            // Assert
            Assert.NotNull(service.GetErrorResponses());
            Assert.NotEmpty(service.GetErrorResponses());
        }

        [Fact]
        public void AddErrorResponse_ShouldThrow()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<IErrorResponseService>>();
            var service = new ErrorResponseService(mockLogger.Object);
            var response = ErrorResponse
                .CreateErrorResponse()
                .ForException<OperationCanceledException>()
                .WithReturnType<NotFoundDetails>();

            // Act
            service.AddErrorResponse(response);
            var action = new Action(() => service.AddErrorResponse(response));

            // Assess
            TestOutputHelper.WriteLine($"Error Responses: {response.ToJson()}");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        public static IEnumerable<object[]> GetValidErrorResponses()
        {
            return TestHelper.GetValidErrorResponses().ToList();
        }
    }
}