using WebApi.Domain.Exceptions;
using WebApi.Services.Common.Security;
using WebApi.Services.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class ProductExtraController : ControllerBase
{
    private readonly ISender _sender;

    public ProductExtraController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> List(int page = 1, int limit = 10)
    {
        try
        {
            return Ok(await _sender.Send(new ListProductExtraRequest
            {
                Page = page,
                Limit = limit
            }));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductExtraRequest request)
    {
        try
        {
            int productExtraId = await _sender.Send(request);
            return Ok(await _sender.Send(new GetProductExtraByIdRequest{Id = productExtraId}));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProductExtraRequest request)
    {
        try
        {
            int productExtraId = await _sender.Send(request);
            return Ok(await _sender.Send(new GetProductExtraByIdRequest{Id = productExtraId}));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}