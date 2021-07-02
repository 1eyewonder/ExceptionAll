﻿using ExceptionAll.Details;
using ExceptionAll.Dtos;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace ExceptionAll.Tests.Details
{
    public class InternalServerErrorDetailsTests : TestBase
    {
        private const string Title = "Internal Server Error";
        private const string Message = "Test Message";
        private readonly List<ErrorDetail> _errorDetails = new();

        public InternalServerErrorDetailsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _errorDetails.Add(new ErrorDetail(Title, Message));
        }

        [Fact]
        public void ActionContextConstructor_ShouldSuccessfullyConstruct1()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();

            // Act
            var test = new InternalServerErrorDetails(mockActionContext.Object);

            // Assess
            TestOutputHelper.WriteLine($"New object {test}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(500, test.Status);
            Assert.NotEqual(Message, test.Detail);
        }

        [Fact]
        public void ActionContextConstructor_ShouldSuccessfullyConstruct2()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();

            // Act
            var test = new InternalServerErrorDetails(mockActionContext.Object, Title);

            // Assess
            TestOutputHelper.WriteLine($"New object {test}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(500, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.NotEqual(Message, test.Detail);
        }

        [Fact]
        public void ActionContextConstructor_ShouldSuccessfullyConstruct3()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();

            // Act
            var test = new InternalServerErrorDetails(mockActionContext.Object, Title, Message);

            // Assess
            TestOutputHelper.WriteLine($"New object {test}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(500, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.Equal(Message, test.Detail);
        }

        [Fact]
        public void ActionContextConstructor_ShouldSuccessfullyConstruct4()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockActionContext();

            // Act
            var test = new InternalServerErrorDetails(mockActionContext.Object, Title, Message, _errorDetails);
            test.Extensions.TryGetValue("Errors", out var value);

            // Assess
            TestOutputHelper.WriteLine($"New object {test}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(500, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.Equal(Message, test.Detail);
            Assert.Equal(_errorDetails, value);
        }

        [Fact]
        public void ExceptionContextConstructor_ShouldSuccessfullyConstruct1()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockExceptionContext();

            // Act
            var test = new InternalServerErrorDetails(mockActionContext.Object);

            // Assess
            TestOutputHelper.WriteLine($"New object {test}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(500, test.Status);
            Assert.NotEqual(Message, test.Detail);
        }

        [Fact]
        public void ExceptionContextConstructor_ShouldSuccessfullyConstruct2()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockExceptionContext();

            // Act
            var test = new InternalServerErrorDetails(mockActionContext.Object, Title);

            // Assess
            TestOutputHelper.WriteLine($"New object {test}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(500, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.NotEqual(Message, test.Detail);
        }

        [Fact]
        public void ExceptionContextConstructor_ShouldSuccessfullyConstruct3()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockExceptionContext();

            // Act
            var test = new InternalServerErrorDetails(mockActionContext.Object, Title, Message);

            // Assess
            TestOutputHelper.WriteLine($"New object {test}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(500, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.Equal(Message, test.Detail);
        }

        [Fact]
        public void ExceptionContextConstructor_ShouldSuccessfullyConstruct4()
        {
            // Arrange
            var mockActionContext = TestHelper.GetMockExceptionContext();

            // Act
            var test = new InternalServerErrorDetails(mockActionContext.Object, Title, Message, _errorDetails);
            test.Extensions.TryGetValue("Errors", out var value);

            // Assess
            TestOutputHelper.WriteLine($"New object {test}");

            // Assert
            Assert.NotNull(test);
            Assert.Equal(500, test.Status);
            Assert.Equal(Title, test.Title);
            Assert.Equal(Message, test.Detail);
            Assert.Equal(_errorDetails, value);
        }
    }
}