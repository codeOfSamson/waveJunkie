using Microsoft.AspNetCore.Mvc;
using SurfForecastApi.Models;
using SurfForecastApi.Services;

namespace SurfForecastApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaritimeController : ControllerBase
{
    private readonly XweatherClient _xweather;

    public MaritimeController(XweatherClient xweather)
    {
        _xweather = xweather;
    }

    [HttpGet]
    public async Task<ActionResult<MaritimeResponse>> Get([FromQuery] double lat, [FromQuery] double lon)
    {
        var result = await _xweather.GetMaritimeDataAsync(lat, lon);

        if (result == null) return NotFound("Could not fetch marine data.");

        return Ok(result);
    }
}
