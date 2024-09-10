using WebApi.Services.Common.Security;
using WebApi.Services.Stores;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class StoreController : ControllerBase
{
    private readonly ISender _sender;

    public StoreController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _sender.Send(new GetStoreRequest()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("region")]
    public async Task<IActionResult> Region(string? cityState)
    {
        try
        {
            return Ok(await _sender.Send(new ListStore
            {
                CityState = cityState,
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("hostname")]
    public async Task<IActionResult> GetByHostname(string hostname)
    {
        try
        {
            return Ok(await _sender.Send(new GetStoreByHostnameRequest{Hostname = hostname}));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e);
        }
    }
    
   [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateStoreRequest request)
    {
        try
        {
            return Ok(await _sender.Send(request));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Route("address")]
    public async Task<IActionResult> SaveAddress([FromBody] UpdateStoreAddressRequest request)
    {
        try
        {
            return Ok(await _sender.Send(request));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("address")]
    public async Task<IActionResult> GetAddress()
    {
        try
        {
            return Ok(await _sender.Send(new GetStoreAddress()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("settings")]
    public async Task<IActionResult> GetSettings()
    {
        try
        {
            return Ok(await _sender.Send(new GetStoreSettings()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut]
    [Route("settings")]
    public async Task<IActionResult> Settings([FromBody] UpdateStoreSettings request)
    {
        try
        {
            return Ok(await _sender.Send(request));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
}