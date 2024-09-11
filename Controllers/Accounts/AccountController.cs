using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Accounts;

namespace WebApi.Controllers.Accounts;

[AllowAnonymous]
[ApiController]
[Route("v1/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly ISender _sender;
    public AccountController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> DocumentExists(string? document)
    {
        try
        {
            return Ok(await _sender.Send(new DocumentExists
            {
                Document = document
            }));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] Login request)
    {
        try
        {
            return Ok(await _sender.Send(request));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetUserByLogin([FromBody] GetUserByLogin request)
    {
        try
        {
            return Ok(await _sender.Send(request));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}