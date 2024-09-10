using WebApi.Services.Common.Security;
using WebApi.Services.OpeningHours;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class OpeningHourController : ControllerBase
{
    private readonly ISender _sender;

    public OpeningHourController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _sender.Send(new GetOpeningHourRequest()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> Count()
    {
        try
        {
            return Ok(await _sender.Send(new GetCountRequest()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateOpeningHourRequest request)
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

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteOpeningHourRequest request)
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