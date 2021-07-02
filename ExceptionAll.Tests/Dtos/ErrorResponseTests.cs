using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Services;
using ExceptionAll.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using FluentValidation;
using Xunit;
using Xunit.Abstractions;

namespace ExceptionAll.Tests.Dtos
{
    public class ErrorResponseTests : TestBase
    {
        public ErrorResponseTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [MemberData(nameof(GetValidErrorResponses))]
        public void ErrorResponse_ShouldBeValid(ErrorResponse response)
        {
            // Arrange & Act
            var test = new ErrorResponseValidator().Validate(response);

            // Assess
            foreach (var validationFailure in test.Errors)
            {
                TestOutputHelper.WriteLine($"Validation failure: {validationFailure.ErrorMessage}");
            }

            // Assert
            Assert.True(test.IsValid);
        }

        [Theory]
        [MemberData(nameof(GetInvalidErrorResponses))]
        public void ErrorResponse_ShouldNotBeValid(ErrorResponse response)
        {
            // Arrange & Act
            var test = new ErrorResponseValidator().Validate(response);

            // Assess
            foreach (var validationFailure in test.Errors)
            {
                TestOutputHelper.WriteLine($"Validation failure: {validationFailure.ErrorMessage}");
            }

            // Assert
            Assert.True(!test.IsValid);
        }

        public static IEnumerable<object[]> GetValidErrorResponses()
        {
            return new List<object[]>
            {
                // Every property populated
                new object[]{new ErrorResponse
                {
                    ErrorTitle = "Test",
                    ExceptionType = typeof(Exception),
                    DetailsType = typeof(BadRequestDetails),
                    LogAction =  (x) => new Mock<ActionResultService>().Object.Logger.LogDebug(x, "Test")
                }},

                // No title
                new object[]{new ErrorResponse
                {
                    ExceptionType = typeof(Exception),
                    DetailsType = typeof(BadRequestDetails),
                    LogAction =  (x) => new Mock<ActionResultService>().Object.Logger.LogDebug(x, "Test")
                }},

                // No log action
                new object[]{new ErrorResponse
                {
                    ErrorTitle = "Test",
                    ExceptionType = typeof(Exception),
                    DetailsType = typeof(BadRequestDetails)
                }},

                // Only details type
                new object[]{new ErrorResponse
                {
                    DetailsType = typeof(NotFoundDetails)
                }},

                new object[]{new ErrorResponse
                {
                    ExceptionType = typeof(ArgumentException),
                    DetailsType = typeof(NotFoundDetails)
                }},

                new object[]{new ErrorResponse
                {
                    ExceptionType = typeof(ArgumentNullException),
                    DetailsType = typeof(NotFoundDetails)
                }},

                new object[]{new ErrorResponse
                {
                    ExceptionType = typeof(OperationCanceledException),
                    DetailsType = typeof(NotFoundDetails)
                }},

                new object[]{new ErrorResponse
                {
                    ExceptionType = typeof(ValidationException),
                    DetailsType = typeof(NotFoundDetails)
                }},
            };
        }

        public static IEnumerable<object[]> GetInvalidErrorResponses()
        {
            return new List<object[]>
            {
                new object[]{new ErrorResponse
                {
                    ExceptionType = typeof(string),
                    DetailsType = typeof(ErrorResponse),
                }},
            };
        }
    }
}