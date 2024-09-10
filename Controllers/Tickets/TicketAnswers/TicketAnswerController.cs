using WebApi.Services.Tickets.TicketAnswers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Tickets.TicketAnswers;

[AllowAnonymous]
[ApiController]
[Route("v1/[controller]")]
public class TicketAnswerController : ControllerBase
{
    private readonly ISender _sender;

    public TicketAnswerController(ISender sender) => _sender = sender;
    
    [HttpPost]
    public async Task<IActionResult> CreateTicketAnswer([FromBody] CreateTicketAnswerRequest request)
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