using WebApi.Services.Common.Security;
using WebApi.Services.Stores;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]/[action]")]
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
   
    // [HttpGet]
    // [AllowAnonymous]
    // [Route("hostname")]
    // public async Task<IActionResult> GetByHostname(string hostname)
    // {
    //     try
    //     {
    //         return Ok(await _sender.Send(new GetStoreByHostnameRequest { Hostname = hostname }));
    //     }
    //     catch (InvalidOperationException e)
    //     {
    //         return BadRequest(e);
    //     }
    // }

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

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStore request)
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
    public async Task<IActionResult> Address([FromBody] UpdateStoreAddressRequest request)
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
    public async Task<IActionResult> Address()
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
    public async Task<IActionResult> Settings()
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