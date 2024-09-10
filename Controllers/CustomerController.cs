using WebApi.Services.Collaborators;
using WebApi.Services.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Exceptions;
using WebApi.Services.Customers;
using WebApi.Services.Rewards;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ISender _sender;

    public CustomerController(ISender sender) => _sender = sender;
    
    [HttpGet]
    public async Task<IActionResult> List(string? name, string? phone)
    {
        try
        {
            return Ok(await _sender.Send(new ListCustomer
            {
                Name = name,
                Phone = phone
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
}