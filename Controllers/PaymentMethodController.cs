using WebApi.Services.Common.Security;
using WebApi.Services.Stores;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.OpeningHours;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class PaymentMethodController : ControllerBase
{
    private readonly ISender _sender;

    public PaymentMethodController(ISender sender) => _sender = sender;


    [HttpGet]
    public async Task<IActionResult> GetPaymentMethod()
    {
        try
        {
            return Ok(await _sender.Send(new GetPaymentMethodsRequest()));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaymentMethod([FromBody] CreateOrUpdatePaymentMethodRequest request)
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