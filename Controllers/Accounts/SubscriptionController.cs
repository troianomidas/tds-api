using WebApi.Domain.Constants;
using WebApi.Domain.Exceptions;
using WebApi.Integrations.Queues;
using WebApi.Services.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[AllowAnonymous]
[ApiController]
[Route("v1/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IQueue _queue;
    
    public SubscriptionController(ISender sender, IQueue queue)
    {
        _queue = queue;
        _sender = sender;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _sender.Send(new GetSubscription()));
        }
        catch (DataNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Route("billing")]
    public async Task<IActionResult> Billing([FromBody] CreateSubscriptionBilling request)
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
    [Route("expired")]
    public async Task<IActionResult> Expired()
    {
        try
        {
            return Ok(await _sender.Send(new SubscriptionExpireValidation()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Route("billing/webhook")]
    public async Task<IActionResult> Webhook([FromForm] Guid notification)
    {
        await _queue.SendMessageAsync(QueueConst.WebhookSubscriptionBilling, notification.ToString());
        return Ok();
    }
}