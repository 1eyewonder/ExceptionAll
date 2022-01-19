using System;
using ExceptionAll.Details;
using System.Collections.Generic;
using ExceptionAll.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Xunit;
using Xunit.Abstractions;

namespace ExceptionAll.Tests.Details
{
    public class UnauthorizedDetailsTests : TestBase
    {
        private const string Title = "Unauthorized";
        private const string Message = "Test Message";
        private readonly List<ErrorDetail> _errorDetails = new();

        public UnauthorizedDetailsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _errorDetails.Add(new ErrorDetail(Title, Message));
        }

        [Fact]
        public void ActionContextConstructor_ShouldSuccessfullyConstruct1()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();

            // Act
            var test = new UnauthorizedDetails(mockActionContext.Object);

            // Assess
            TestOutputHelper.WriteLine($"New object {test.ToJson()}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(401, test.Status);
            Assert.NotEqual(Message, test.Detail);
        }

        [Fact]
        public void ActionContextConstructor_ShouldSuccessfullyConstruct2()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();

            // Act
            var test = new UnauthorizedDetails(mockActionContext.Object, Title);

            // Assess
            TestOutputHelper.WriteLine($"New object {test.ToJson()}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(401, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.NotEqual(Message, test.Detail);
        }

        [Fact]
        public void ActionContextConstructor_ShouldSuccessfullyConstruct3()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();

            // Act
            var test = new UnauthorizedDetails(mockActionContext.Object, Title, Message);

            // Assess
            TestOutputHelper.WriteLine($"New object {test.ToJson()}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(401, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.Equal(Message, test.Detail);
        }

        [Fact]
        public void ActionContextConstructor_ShouldSuccessfullyConstruct4()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();

            // Act
            var test = new UnauthorizedDetails(mockActionContext.Object, Title, Message, _errorDetails);
            test.Extensions.TryGetValue("Errors", out var value);

            // Assess
            TestOutputHelper.WriteLine($"New object {test.ToJson()}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(401, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.Equal(Message, test.Detail);
            Assert.Equal(_errorDetails, value);
        }

        [Fact]
        public void ActionContextConstructor_ShouldThrowArgumentNullException()
        {
            // Arrange
            ActionContext context = null;

            // Act
            var action = new Func<UnauthorizedDetails>(() => new UnauthorizedDetails(context));

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ExceptionContextConstructor_ShouldSuccessfullyConstruct1()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockExceptionContext<Exception>();

            // Act
            var test = new UnauthorizedDetails(mockActionContext.Object);

            // Assess
            TestOutputHelper.WriteLine($"New object {test.ToJson()}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(401, test.Status);
            Assert.NotEqual(Message, test.Detail);
        }

        [Fact]
        public void ExceptionContextConstructor_ShouldSuccessfullyConstruct2()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockExceptionContext<Exception>();

            // Act
            var test = new UnauthorizedDetails(mockActionContext.Object, Title);

            // Assess
            TestOutputHelper.WriteLine($"New object {test.ToJson()}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(401, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.NotEqual(Message, test.Detail);
        }

        [Fact]
        public void ExceptionContextConstructor_ShouldSuccessfullyConstruct3()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockExceptionContext<Exception>();

            // Act
            var test = new UnauthorizedDetails(mockActionContext.Object, Title, Message);

            // Assess
            TestOutputHelper.WriteLine($"New object {test.ToJson()}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(401, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.Equal(Message, test.Detail);
        }

        [Fact]
        public void ExceptionContextConstructor_ShouldSuccessfullyConstruct4()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockExceptionContext<Exception>();

            // Act
            var test = new UnauthorizedDetails(mockActionContext.Object, Title, Message, _errorDetails);
            test.Extensions.TryGetValue("Errors", out var value);

            // Assess
            TestOutputHelper.WriteLine($"New object {test.ToJson()}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(401, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.Equal(Message, test.Detail);
            Assert.Equal(_errorDetails, value);
        }

        [Fact]
        public void ExceptionContextConstructor_ShouldThrowArgumentNullException()
        {
            // Arrange
            ExceptionContext context = null;

            // Act
            var action = new Func<UnauthorizedDetails>(() => new UnauthorizedDetails(context));

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}