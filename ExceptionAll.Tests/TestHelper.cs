using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Interfaces;
using ExceptionAll.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExceptionAll.Tests
{
    public static class TestHelper
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });
        }

        public static Mock<ActionContext> GetMockActionContext(HttpContext context = null)
        {
            var mockActionContext = new Mock<ActionContext>(
                context ?? GetMockHttpContext(),
                new RouteData(),
                new ActionDescriptor());
            return mockActionContext;
        }

        public static Mock<Exception> GetMockException()
        {
            var mockException = new Mock<Exception>();
            mockException.Setup(x => x.StackTrace)
                .Returns("Test Stacktrace");
            mockException.Setup(x => x.Message)
                .Returns("Test message");
            mockException.Setup(x => x.Source)
                .Returns("Test source");

            return mockException;
        }

        public static Mock<ExceptionContext> GetMockExceptionContext(HttpContext context = null, Exception exception = null)
        {
            var mockExceptionContext = new Mock<ExceptionContext>(
                GetMockActionContext(context).Object, new List<IFilterMetadata>());
            mockExceptionContext.Setup(x => x.Exception)
                .Returns(exception ?? GetMockException().Object);

            return mockExceptionContext;
        }

        public static DefaultHttpContext GetMockHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("x-correlation-id", Guid.NewGuid().ToString());
            context.TraceIdentifier = Guid.NewGuid().ToString();
            return context;
        }

        public static Mock<ActionResultService>GetMockActionResultService()
        {
            var mockActionLogger = new Mock<ILogger<IActionResultService>>();
            var mockErrorResponseService = new Mock<ErrorResponseService>();
            return new Mock<ActionResultService>(
                    mockActionLogger.Object, 
                    mockErrorResponseService.Object);
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