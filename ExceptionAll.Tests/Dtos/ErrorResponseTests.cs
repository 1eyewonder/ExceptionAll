using ExceptionAll.Dtos;
using ExceptionAll.Validation;
using System.Collections.Generic;
using System.Linq;
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
            var mockActionResultService = TestHelper.GetMockActionResultService();
            return TestHelper.GetValidErrorResponses(mockActionResultService.Object).ToList();
        }

        public static IEnumerable<object[]> GetInvalidErrorResponses()
        {
            var mockActionResultService = TestHelper.GetMockActionResultService();
            return TestHelper.GetInvalidErrorResponses(mockActionResultService.Object).ToList();
        }
    }
}