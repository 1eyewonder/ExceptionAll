namespace ExceptionAll.Tests.Unit.Helpers;

public static class ContextSubstitutes
{
    public static ExceptionContext GetExceptionContext(HttpContext? context = null)
    {
        var actionContext = GetActionContextSub(context);
        return Substitute.For<ExceptionContext>(actionContext, new List<IFilterMetadata>());
    }

    public static ActionContext GetActionContextSub(HttpContext? context = null)
    {
        var httpContext = GetHttpContext(context);
        var routeData   = Substitute.For<RouteData>();
        var descriptor  = Substitute.For<ActionDescriptor>();

        return Substitute.For<ActionContext>(httpContext, routeData, descriptor);
    }

    public static HttpContext GetHttpContext(HttpContext? context = null)
    {
        var httpContext = context ?? new DefaultHttpContext();
        httpContext.Request.Method = "GET";

        httpContext.Request.Headers.Add(
            "x-correlation-id",
            Guid.NewGuid()
                .ToString());

        httpContext.TraceIdentifier = Guid.NewGuid()
                                          .ToString();

        httpContext.Request.Path = "/api/Controller/Action";

        return httpContext;
    }
}