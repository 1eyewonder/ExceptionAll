using System.Net;
using Example.Shared;
using ExceptionAll;
using ExceptionAll.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Example.Server.Controllers;

[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(ExceptionFilter))]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IActionResultService _actionResultService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IActionResultService actionResultService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _actionResultService = actionResultService ?? throw new ArgumentNullException(nameof(actionResultService));
    }

    [HttpGet("api/success")]
    public async Task<IActionResult> GetAllSuccess()
    {
        await Task.Delay(0);
        var rng = new Random();
        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                               {
                                   Date         = DateTime.Now.AddDays(index),
                                   TemperatureC = rng.Next(-20, 55),
                                   Summary      = Summaries[rng.Next(Summaries.Length)]
                               })
                               .ToArray();

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiErrorDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        await Task.Delay(0);
        var rng = new Random();
        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
                               .ToArray();
        throw new Exception("This is simulating an uncaught exception");
    }

    [HttpGet]
    [Route("api/GetNullRefError")]
    public async Task<IActionResult> GetNullRefError(string param, string otherParam)
    {
        param = null;
        await Task.Delay(0);
        throw new ArgumentNullException(nameof(param));
    }

    // If the developer doesn't want to use the template in all instances,
    // wrapping the code in try catch will let you use your own logic
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

    // If the developer needs to manually error handle, they can call
    // the 'GetResponse' manually
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
    
    [HttpPost]
    [Route("api/AddForecast")]
    [ProducesResponseType(typeof(BadRequestDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InternalServerErrorDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddForecast([FromBody] WeatherForecast forecast)
    {
        await Task.Delay(0);
        return Created(string.Empty, forecast);
    }
}