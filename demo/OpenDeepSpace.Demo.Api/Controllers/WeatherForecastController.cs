using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;

namespace OpenDeepSpace.Demo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        [AutomaticInjection]
        private readonly ILogger<WeatherForecastController> _logger;

        [AutomaticInjection(ImplementationType = typeof(IOptions<MvcOptions>))]
        private readonly MvcOptions mvcOptions;

        public WeatherForecastController(/*ILogger<WeatherForecastController> logger*/)
        {
            /*_logger = logger;*/
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}