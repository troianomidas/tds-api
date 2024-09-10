using WebApi.Services.Common.Security;
using WebApi.Services.Stores;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class DeliveryAreaController : ControllerBase
{
    private readonly ISender _sender;

    public DeliveryAreaController(ISender sender) => _sender = sender;
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _sender.Send(new GetStoreDeliveryRequest()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpGet]
    [Route("areas")]
    public async Task<IActionResult> Areas()
    {
        try
        {
            return Ok(await _sender.Send(new GetDeliveryAreas()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateStoreDeliveryRequest request)
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
    
    [HttpPut]
    [Route("areas")]
    public async Task<IActionResult> Put([FromBody] CreateOrUpdateDeliveryAreas request)
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