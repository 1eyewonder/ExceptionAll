using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;

namespace ExceptionAll.Tests
{
    public static class TestHelper
    {
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
    }
}