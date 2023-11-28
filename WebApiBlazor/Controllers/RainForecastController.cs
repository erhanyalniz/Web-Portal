using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace WebApiBlazor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RainForecastController : ControllerBase
    {
        private static readonly string[] RainTypes = new[]
        {
            "Light Rain", "Heavy Rain", "Thunderstorm", "Hail", "Mild", "Freezing Rain"
        };

        [HttpGet(Name = "GetRainForecast")]
        public IEnumerable<RainForecast> GetRainForecast()
        {
            // Implement logic to generate and return rain forecasts
            return Enumerable.Range(1, 5).Select(index => new RainForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                ChanceOfRain = new Random().Next(0, 100),
                RainType = RainTypes[new Random().Next(RainTypes.Length)]
            })
            .ToArray();
        }
    }
}
