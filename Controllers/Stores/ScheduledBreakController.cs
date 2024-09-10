using WebApi.Services.Common.Security;
using WebApi.Services.OpeningHours;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.ScheduledBreaks;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class ScheduledBreakController : ControllerBase
{
    private readonly ISender _sender;

    public ScheduledBreakController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _sender.Send(new GetScheduledBreak()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateScheduledBreak request)
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
    public async Task<IActionResult> Delete(DeleteScheduledBreak request)
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