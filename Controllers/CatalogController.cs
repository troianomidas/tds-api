using WebApi.Domain.Exceptions;
using WebApi.Services.Common.Security;
using WebApi.Services.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ISender _sender;

    public CatalogController(ISender sender) => _sender = sender;

    [HttpGet]
    [AllowAnonymous]
    [Route("highlights")]
    public async Task<IActionResult> Highlights()
    {
        try
        {
            return Ok(await _sender.Send(new ListProductsHighlights()));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> List(string? filter, int page = 1, int limit = 10)
    {
        try
        {
            return Ok(await _sender.Send(new ListProductsRequest
            {
                Filter = filter,
                Page = page,
                Limit = limit
            }));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            return Ok(await _sender.Send(new GetProductByIdRequest
            {
                ProductId = id
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateProductRequest request)
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

    [HttpPost]
    [Route("Duplicate")]
    public async Task<IActionResult> Duplicate([FromBody] DuplicateProductRequest request)
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
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteProductRequest request)
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