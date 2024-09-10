using WebApi.Domain.Exceptions;
using WebApi.Services.Geolocation;
using WebApi.Services.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[AllowAnonymous]
[ApiController]
[Route("v1/[controller]")]
public class GeolocationController : ControllerBase
{
    private readonly ISender _sender;

    public GeolocationController(ISender sender) => _sender = sender;

    [HttpGet]
    [Route("cities")]
    public async Task<IActionResult> Cities(string name)
    {
        try
        {
            return Ok(await _sender.Send(new GetCitiesByName
            {
                Name = name
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("city-nearby")]
    public async Task<IActionResult> CityNearby(string lat, string lng)
    {
        try
        {
            return Ok(await _sender.Send(new GetCitiesNearbyByLatLng
            {
                Lat = lat,
                Lng = lng,
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
}