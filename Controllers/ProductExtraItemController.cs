using WebApi.Domain.Exceptions;
using WebApi.Services.Common.Security;
using WebApi.Services.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class ProductExtraItemController : ControllerBase
{
    private readonly ISender _sender;

    public ProductExtraItemController(ISender sender) => _sender = sender;

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProductExtraItemRequest request)
    {
        try
        {
            return Ok(await _sender.Send(request));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}