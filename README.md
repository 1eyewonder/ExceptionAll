# ExceptionAll

Package | Version | Downloads
-----|------|-------
<b>ExceptionAll</b> | [![Nuget version](https://img.shields.io/nuget/v/ExceptionAll)](https://www.nuget.org/packages/ExceptionAll/) | [![Nuget downloads](https://img.shields.io/nuget/dt/ExceptionAll)](https://www.nuget.org/packages/ExceptionAll/)
<b>ExceptionAll.Abstractions</b> | [![Nuget version](https://img.shields.io/nuget/v/ExceptionAll.Abstractions)](https://www.nuget.org/packages/ExceptionAll.Abstractions/) | [![Nuget downloads](https://img.shields.io/nuget/dt/ExceptionAll.Abstractions)](https://www.nuget.org/packages/ExceptionAll.Abstractions/)
<b>ExceptionAll.Client</b> | [![Nuget version](https://img.shields.io/nuget/v/ExceptionAll.Client)](https://www.nuget.org/packages/ExceptionAll.Client/) | [![Nuget downloads](https://img.shields.io/nuget/dt/ExceptionAll.Client)](https://www.nuget.org/packages/ExceptionAll.Client/)


## Table of Contents
  - [Summary](#summary)
  - [Setup](#setup)
    - [Server Side](#server-side)
    - [Client Side](#client-side)
  - [Example Code](#example-code)
  - [Swagger Examples](#swagger-examples)
  - [Extending ExceptionAll](#extending-exceptionall)  

________
## Summary
ExceptionAll is a library for adding global error handling to Web API solutions using .NET Core. Its goals are to:
1. Reduce code noise by reducing the need for 'try-catch' blocks in code
2. Provide a single source of responsibility for configuring and maintaining API error handling logic
3. Allow for the customization of error response data and logging actions
4. Reduce the amount of time needed to set up Swagger documentation
5. Simplify Http client serialization and deserialization

_______
## Setup

<i>Note: ExceptionAll offers both front and backend solutions but they do not need to be mutally exclusive. Please read over what each solution does and how it is applicable to your problem/application</i>

### Server Side
1. Install ExceptionAll & ExceptionAll.Abstractions nuget packages
2. Create an ExceptionAll configuration class which implements the <b>IExceptionAllConfiguration</b> interface. Below are some examples various examples but are not required
   1. ErrorResponses
        - A list of the types of error responses for specific error types encountered.
   2. ContextConfiguration
        - Allows the developer to extend the standard response object by adding details from the HttpContext. Options become limitless since custom headers are accessible. 
        - As the context properties are updated, the Swagger documentation should also be updated as well.

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

3. In Program.cs:

    ```csharp
    using ExceptionAll.Helpers;

    builder.Services
           .AddExceptionAll<ExceptionAllConfiguration>()
           .WithExceptionAllSwaggerExamples(); // optional, adds the default Swagger response examples
    ```

### Client Side
1. Install both ExceptionAll.Client & ExceptionAll.Abstractions nuget packages
2. Add the following code within your Program.cs file in order to inject the applicable services


    ```csharp
    builder.Services.AddExceptionAllClientServices();
    builder.Services.AddHttpClient();
    ```
    a. In client use, you will use 'IExceptionAllClientFactory' which is a wrapper around the standard .NET IHttpClientFactory interface. If clients are needed to be configured differently for different endpoints, simply add additional HttpClients with the applicable configurations.
_______
## Example Code
1. The default API response provided by ExceptionAll. This simulates an uncaught exception in your API code. This response will also be returned for specific exception types not initially considered during configuration.
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
   2. API Response
    
        ![alt text](ReadMeImages\v4\ApiControllerStandardResponse.PNG)

2. This example shows catching an exception configured in the configuration class. (See above configuration code)
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
    1. API Response. The properties match what we see in our configuration, seen further up on the page.

        ![alt text](ReadMeImages\v4\ArgumentNullRefResponse.PNG)

3. There may be times where an ExceptionAll response is undesired. To get a non-ExceptionAll response, just wrap the controller/endpoint code with a standard 'try-catch' block and return the new, desired object.
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
   1. API Response
   
        ![alt text](ReadMeImages\v4\NonExceptionAllResponse.PNG)

4. This example covers manual response generation, for times developers want to return caught exceptions with a special message and/or a surface list of errors to the user
   1. API Controller code
      1. Make sure to inject the 'IActionResultService' into your controller constructor

    ```csharp
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IActionResultService actionResultService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _actionResultService = actionResultService ?? throw new ArgumentNullException(nameof(actionResultService));
        }

        [HttpGet]
        [Route("api/GetSomething")]
        public async Task<IActionResult> GetSomethingWithQuery([FromQuery] string test)
        {
            await Task.Delay(0);

            var errors = new List<ErrorDetail>
            {
                new("Error #1", "Something wrong happened here"),
                new("Error #2", "Something wrong happened there")
            };

            return _actionResultService.GetResponse<NotFoundDetails>(
                ControllerContext,
                $"No item exists with name of {test}",
                errors);
        }
    ```
    1. API Response
   
        ![alt text](ReadMeImages\v4\ManuallyReturnedResponse.PNG)

_______

## Swagger Examples

The following objects are provided out of the box to provided to handle common API errors as well as give Swagger documentation examples.
1. BadGatewayDetails
2. BadRequestDetails
3. ConflictDetails
4. ForbiddenDetails
5. InternalServerErrorDetails
6. NotFoundDetails
7. NotImplementedDetails
8. RequestTimeoutDetails
9. ServiceUnavailableDetails
10. TooManyRequestDetails
11. UnauthorizedDetails
12. UnsupportedMediaTypeDetails

In order to provide the Swagger examples, add attributes with the return object as well as the HTTP status codes your endpoint handles.

```csharp
    [HttpGet]
    [Route("api/GetSomething")]
    [ProducesResponseType(typeof(BadRequestDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(InternalServerErrorDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSomethingWithQuery([FromQuery] string test)
    {
        await Task.Delay(0);

        var errors = new List<ErrorDetail>
        {
            new("Error #1", "Something wrong happened here"),
            new("Error #2", "Something wrong happened there")
        };

        return _actionResultService.GetResponse<NotFoundDetails>(
            ControllerContext,
            $"No item exists with name of {test}",
            errors);
    }
```

The above code should give you the following Swagger response examples:

![alt text](ReadMeImages\v4\SwaggerExamples.PNG)

_______

## Extending ExceptionAll
ExceptionAll provides some standard detail objects out of the box, one of which is shown below. If you, as a developer, need to extend the library
and create additional detail types, follow the below example as a template and implement the IExceptionAllDetails interface on your custom object.

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

To create a Swagger response example for the new object, create a class similar to the following. Utilize unit test libraries to mock a more detailed HttpContext, if desired.

```csharp
public class BadGatewayDetailsExample : IExamplesProvider<BadGatewayDetails>
{
    private readonly IContextConfigurationService _contextConfigurationService;

    public BadGatewayDetailsExample(IContextConfigurationService contextConfigurationService)
    {
        _contextConfigurationService = contextConfigurationService;
    }

    public BadGatewayDetails GetExamples()
    {
        return new BadGatewayDetails()
        {
            Message = "Oops, there was an error",
            ContextDetails = _contextConfigurationService.GetContextDetails(
                new DefaultHttpContext(),
                new List<ErrorDetail>
                {
                    new("Error!", "Something broke")
                })
        };
    }
}
```
