using System.Net;
using WebApi.Domain.Exceptions;
using WebApi.Services.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ISender _sender;

    public OrderController(ISender sender) => _sender = sender;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            return Ok(await _sender.Send(new GetOrderRequest
            {
                OrderId = id
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("track")]
    [AllowAnonymous]
    public async Task<IActionResult> Track(long trackId)
    {
        try
        {
            return Ok(await _sender.Send(new GetOrderRequest
            {
                TrackId = trackId
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> List(int number, int status, DateTime from, DateTime to)
    {
        try
        {
            return Ok(await _sender.Send(new ListOrderRequest
            {
                Number = number,
                Status = status,
                From = from,
                To = to,
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("workflow")]
    public async Task<IActionResult> Workflow()
    {
        try
        {
            return Ok(await _sender.Send(new ListConveyorBeltRequest()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        try
        {
            return Ok(await _sender.Send(request));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new { Error = e.Message, Exception = e.InnerException?.Message });
        }
    }

    [HttpPut]
    [Route("status")]
    public async Task<IActionResult> Status([FromBody] UpdateOrderStatusRequest request)
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