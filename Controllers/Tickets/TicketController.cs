using WebApi.Services.Tickets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Tickets;

[AllowAnonymous]
[ApiController]
[Route("v1/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ISender _sender;

    public TicketController(ISender sender) => _sender = sender;
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTicketById(int id)
    {
        try
        {
            return Ok(await _sender.Send(new GetTicketByIdRequest
            {
                Id = id
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> ListTicketsByStoreId()
    {
        try
        {
            return Ok(await _sender.Send(new ListTicketsByStoreIdRequest()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request)
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
    public async Task<IActionResult> UpdateTicket([FromBody] UpdateTicketRequest request)
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