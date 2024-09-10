using WebApi.Services.ExpiratedPoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Microsoft.AspNetCore.Authorization.AllowAnonymous]
[ApiController]
[Route("v1/[controller]")]
public class ExpiratedPointsController : ControllerBase
{
    private readonly ISender _sender;

    public ExpiratedPointsController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> ListExpiratedPoints()
    {
        try
        {
            return Ok(await _sender.Send(new ListExpiratedPointsRequest()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateInvalidPoints([FromBody] UpdateInvalidPoints request)
    {
        try
        {
            return Ok(await _sender.Send(request));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e);
        }
    }
}