# ExceptionAll
Lightweight extension for adding structured, global error handling to Web API solutions using .NET Core

# Setup
In Startup.cs add the following namespaces:
```
using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Filters;
using ExceptionAll.Helpers;
using ExceptionAll.Interfaces;
using ExceptionAll.Services;
```

In Startup.cs under 'ConfigureServices':

```csharp
services.AddSingleton<IErrorResponseService, ErrorResponseService>();
services.AddSingleton<IActionResultService, ActionResultService>();

// This will also work for anyone implementing the MVC or REPR patterns
services.AddControllers(x =>
{
    x.Filters.Add(typeof(ExceptionFilter));
})

// This section is optional. I choose to use this option to keep my objects from returning nulls
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
```

In Startup.cs under 'Configure'

```csharp
// Inject the ErrorResponse and ActionResult service interfaces
 public void Configure(IApplicationBuilder app,
            IErrorResponseService errorResponseService,
            IActionResultService actionResultService)
        {
            // Adds a global response for a unique Error type
            // You can call 'AddErrorResponse' for every exception you would like
            // to globally handle
            errorResponseService.AddErrorResponse(new ErrorResponse
            {
                // Error title returned
                ErrorTitle = "Bad Request - Data Annotations",
                
                // Type of error this response handles
                ExceptionType = typeof(System.ComponentModel.DataAnnotations.ValidationException),
                
                // Returned object. Must inherit from Microsoft.AspNetCore.Mvc.ProblemDetails
                // Different 'Detail' objects are used to allow for support in Swagger documentation
                DetailsType = typeof(BadRequestDetails),
                
                // Allows developer to choose what level of logging happens for the 
                // specific exception, if desired. If no desire to log, you don't have
                // to declare an Action   
                LogAction = (e) => actionResultService
                    .Logger
                    .LogDebug(e, e.Message)
            });
```

# Custom Detail Responses
ExceptionAll provides some standard detail objects out of the box. If you as a developer need to extend the library
and create additional detail types, follow the below example as a template:

```csharp
using ExceptionAll.Dtos;
using ExceptionAll.Helpers;

public class BadRequestDetails : ProblemDetails
    {
        // Action context is utitilized during developer caught exceptions
        public BadRequestDetails(ActionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            Title = string.IsNullOrEmpty(title) == false ? title :  "Bad Request";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status400BadRequest;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }

        // Exception context is utilized during global, filter-caught exceptions
        public BadRequestDetails(ExceptionContext context, string title = null, string message = null, List<ErrorDetail> errors = null)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            Title = string.IsNullOrEmpty(title) == false ? title :  "Bad Request";
            Instance = context.HttpContext.Request.Path;
            Status = StatusCodes.Status400BadRequest;
            Detail = string.IsNullOrEmpty(message) == false ? message : "See errors or logs for more details";
            this.AddDefaultExtensionsFromContext(context, errors);
        }
    }
```

You can also add additional properties in the 'Extension' property inherited from ProblemDetails if you don't feel the given properties are enough.

