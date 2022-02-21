namespace ExceptionAll.Tests.Unit;

public static class TestHelper
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj,
                                           Formatting.None,
                                           new JsonSerializerSettings()
                                           {
                                               ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                               Formatting            = Formatting.Indented
                                           });
    }

    public static Mock<ActionContext> GetMockActionContext(HttpContext? context = null)
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

    public static Mock<ExceptionContext> GetMockExceptionContext<T>(HttpContext? context = null, T? exception = null)
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

    public static IActionResultService GetMockActionResultService(IErrorResponseService? service = null)
    {
        var mockErrorResponseService = Substitute.For<IErrorResponseService>();
        return Substitute.For<IActionResultService>();
    }
}