using Microsoft.AspNetCore.Mvc;
using OpenTelemetryTests.Models;

namespace OpenTelemetryTests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelItineraryController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TravelItineraryController> _logger;

        public TravelItineraryController(ILogger<TravelItineraryController> logger)
        {
            _logger = logger;
        }

        [HttpGet(("/weather"))]
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

        [HttpGet("/hello")]
        public IActionResult GetHello()
        {
            _logger.LogInformation("Entering hello world.");
            return Ok("Hello from API.");
        }

        [HttpPost("/flights")]
        public IActionResult BookFlight([FromBody] FlightSegmentRequest request)
        {
            _logger.LogInformation("Entering flight booking logic.");

            if (request.PassengerName == null)
            {
                throw new NullReferenceException("null reference error....take care of the code buddy it can break internet, it can break your app too.");
            }

            if (request.PassengerName == null)
            {
                throw new ArgumentNullException("Argument is null, give proper value.");
            }

            var response = new TravelBookingResponse
            {
                BookingId = Guid.NewGuid().ToString(),
                Status = "Confirmed",
                Message = "Your flight has been booked successfully."
            };

            _logger.LogInformation("Successfully booked flight.");
            return Ok(response);
        }

        [HttpPost("/hotels")]
        public IActionResult BookHotel([FromBody] HotelSegmentRequest request)
        {
            try
            {
                if (request.HotelName == null)
                {
                    throw new NullReferenceException("null reference error....take care of the code buddy it can break internet, it can break your app too.");
                }

                _logger.LogInformation("Entering hotel booking logic.");
                var response = new TravelBookingResponse
                {
                    BookingId = Guid.NewGuid().ToString(),
                    Status = "Confirmed",
                    Message = "Your hotel has been booked successfully."
                };

                _logger.LogInformation("Successfully booked hotel.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }
    }
}
