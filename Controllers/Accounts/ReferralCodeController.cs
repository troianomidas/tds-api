using WebApi.Domain.Constants;
using WebApi.Domain.Exceptions;
// using WebApi.Integrations.Queues;
using WebApi.Services.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.ReferralCodes;

namespace WebApi.Controllers.Accounts;

[AllowAnonymous]
[ApiController]
[Route("v1/[controller]")]
public class ReferralCodeController : ControllerBase
{
    private readonly ISender _sender;
    
    public ReferralCodeController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(string? code)
    {
        try
        {
            return Ok(await _sender.Send(new GetReferralCode
            {
                Code = code
            }));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}