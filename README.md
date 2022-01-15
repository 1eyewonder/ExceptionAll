# ExceptionAll
ExceptionAll is a library for adding structured, global error handling to Web API solutions using .NET Core. It helps reduce code noise by removing the need for 'try-catch' code blocks as well as a singluar source to configure how all exception types are handled, logged, and returned to an API consumer. The package comes with out of the box Swagger example responses to let the developer focus on the code rather than documentation. The configuration of the error responses is also done in a fluent manner, making the code more readable for developers.

## Table of Contents
- [ExceptionAll](#exceptionall)
  - [Table of Contents](#table-of-contents)
  - [Setup](#setup)
  - [Example Code](#example-code)
  - [Extending ExceptionAll](#extending-exceptionall)

## Setup
1. Create an ExceptionAll configuration class
   1. ErrorResponses
        - A list of the types of error responses for specific error types encountered.
   2. ContextConfiguration
        - Allows the developer to extend the standard response object by adding details from the HttpContext. Below are some examples various examples.

   ```csharp
    public class ExceptionAllConfiguration : IExceptionAllConfiguration
    {
        public List<IErrorResponse> ErrorResponses => new()
        {
            ErrorResponse
                .CreateErrorResponse()
                .WithTitle("Argument Null Exception")
                .WithStatusCode(500)
                .WithMessage("The developer goofed")
                .ForException<ArgumentNullException>()
                .WithLogAction((x, e) => x.LogDebug(e, "Oops I did it again"))
        };

        public Dictionary<string, Func<HttpContext, object>>? ContextConfiguration => new()
        {
            { "Path", x => x?.Request.Path.Value ?? string.Empty },
            { "Query", x => x.Request.QueryString.Value ?? string.Empty },
            { "TraceIdentifier", x => x?.TraceIdentifier ?? string.Empty },
            { "LocalIpAddress", x => x?.Connection.LocalIpAddress?.ToString() ?? string.Empty },
            { "CorrelationId", x => x.Request.Headers["x-correlation-id"].ToString() }
        };
    }
   ```

2. In Program.cs add the following namespaces:
   
    ```csharp
    using ExceptionAll.Helpers;
    ```

3. In Program.cs:

    ```csharp
    builder.Services
           .AddExceptionAll<ExceptionAllConfiguration>()
           .WithExceptionAllSwaggerExamples(); // optional, adds the Swagger response examples

    // more dependency injection here
    // ...

    // the standard .NET 6 WebApplicationBuilder
    var app = builder.Build(); 

    app.Services.AddExceptionAll();
    ```

## Example Code
1. The standard API response
   1. API Controller code
   
        ```csharp
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                await Task.Delay(0);
                var rng = new Random();
                var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                }).ToArray();

                throw new Exception("This is simulating an uncaught exception");
            }
        ```
   2. Api Response
    
        ![alt text](ReadMeImages\v4\ApiControllerStandardResponse.PNG)

2. Catching an exception configured in our error response container
   1. API Controller code
   
        ```csharp
            [HttpGet]
            [Route("api/GetNullRefError")]
            public async Task<IActionResult> GetNullRefError(string param, string otherParam)
            {
                param = null;
                await Task.Delay(0);
                throw new ArgumentNullException(nameof(param));
            }
        ```
    2. API Response. Matches what we see in our setup seen further up on the page.

        ![alt text](ReadMeImages\v4\ArgumentNullRefResponse.PNG)

3. Getting around ExceptionAll, since you might have special cases you don't want to see its responses
   1. API Controller code
   ```csharp
    [HttpGet]
    [Route("api/GetWithoutExceptionAllError")]
    public async Task<IActionResult> GetWithoutExceptionAllError()
    {
        await Task.Delay(0);
        try
        {
            throw new Exception("Some exception");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
   ```
   2. API Response
   
        ![alt text](ReadMeImages\v4\NonExceptionAllResponse.PNG)

4. Manual response generation, for times developers want to return caught exceptions
   1. API Controller code

## Extending ExceptionAll
ExceptionAll provides some standard detail objects out of the box, one of which is shown below. If you as a developer need to extend the library
and create additional detail types, follow the below example as a template and implement the IExceptionAllDetails interface on your custom object. The main reason for needing to create your own details is for swagger response object documentation.

```csharp

public class BadGatewayDetails : IExceptionAllDetails
{
    public string Title => GetDetails().Title;
    public int StatusCode => GetDetails().StatusCode;
    public string Message { get; init; } = string.Empty;
    public IReadOnlyDictionary<string, object>? ContextDetails { get; init; }
    public (int StatusCode, string Title) GetDetails()
    {
        return (502, "Bad Gateway");
    }
}
```

