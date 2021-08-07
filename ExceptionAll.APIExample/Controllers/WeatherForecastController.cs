using ExceptionAll.Details;
using ExceptionAll.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionAll.APIExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            })
            .ToArray();
            throw new Exception("This is simulating an uncaught exception");
        }

        // If the developer doesn't want to use the template in all instances,
        // wrapping the code in try catch will let you use your own logic
        [HttpGet]
        [Route("api.GetWithoutExceptionAllError")]
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
        public async Task<IActionResult> GetSomethingWithQuery([FromQuery]string test)
        {
            await Task.Delay(0);
            return _actionResultService.GetResponse<NotFoundDetails>(ControllerContext,
                $"No item exists with name of {test}");
        }
    }
}