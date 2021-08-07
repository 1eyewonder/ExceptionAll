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

        public static Mock<T> GetMockException<T>() where T : Exception
        {
            var mockException = new Mock<T>();
            mockException.Setup(x => x.StackTrace)
                .Returns("Test Stacktrace");
            mockException.Setup(x => x.Message)
                .Returns("Test message");
            mockException.Setup(x => x.Source)
                .Returns("Test source");

            return mockException;
        }

        public static Mock<ExceptionContext> GetMockExceptionContext<T>(HttpContext context = null, T exception = null) 
            where T : Exception
        {
            var mockExceptionContext = new Mock<ExceptionContext>(
                GetMockActionContext(context).Object, 
                new List<IFilterMetadata>());

            mockExceptionContext
                .Setup(x => x.Exception)
                .Returns(exception ?? GetMockException<T>().Object);

            return mockExceptionContext;
        }

        public static DefaultHttpContext GetMockHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("x-correlation-id", Guid.NewGuid().ToString());
            context.TraceIdentifier = Guid.NewGuid().ToString();
            return context;
        }

        public static Mock<ActionResultService> GetMockActionResultService(IErrorResponseService service = null)
        {
            var mockErrorLogger = new Mock<ILogger<IErrorResponseService>>();
            var mockActionLogger = new Mock<ILogger<IActionResultService>>();
            var mockErrorResponseService = new Mock<ErrorResponseService>(mockErrorLogger.Object);
            return new Mock<ActionResultService>(
                    mockActionLogger.Object, 
                    service ?? mockErrorResponseService.Object);
        }

        public static Mock<ErrorResponseService> GetMockErrorResponseService()
        {
            var mockErrorLogger = new Mock<ILogger<IErrorResponseService>>();
            return new Mock<ErrorResponseService>(mockErrorLogger.Object);
        }

        public static IEnumerable<object[]> GetValidErrorResponses()
        {
            return new List<object[]>
            {
                // Every property populated
                new object[]{
                    ErrorResponse
                        .CreateErrorResponse()
                        .WithTitle("Bad Request - Fluent Validation")
                        .ForException<ValidationException>()
                        .WithReturnType<BadRequestDetails>()
                        .WithLogAction((x, e) => x.LogError("Something bad happened", e))
                },

                // No title
                new object[]{
                    ErrorResponse
                        .CreateErrorResponse()
                        .ForException<ValidationException>()
                        .WithReturnType<BadRequestDetails>()
                        .WithLogAction((x, e) => x.LogError("Something bad happened", e))
                },

                // No log action
                new object[]{
                    ErrorResponse
                        .CreateErrorResponse()
                        .WithTitle("Bad Request - Fluent Validation")
                        .ForException<ValidationException>()
                        .WithReturnType<BadRequestDetails>()
                },

                // Only details type
                new object[]{
                    ErrorResponse
                        .CreateErrorResponse()
                        .WithReturnType<BadRequestDetails>()
                },

                // Only creation method
                new object[]{
                    ErrorResponse.CreateErrorResponse()
                },
            };
        }

        public static IEnumerable<object[]> GetInvalidErrorResponses()
        {
            return new List<object[]>
            {
                new object[]{
                    ErrorResponse
                        .CreateErrorResponse()
                        .ForException<Exception>()
                        .WithReturnType<ProblemDetails>()
                }
            };
        }
    }
}