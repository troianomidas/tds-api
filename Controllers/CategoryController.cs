using WebApi.Domain.Exceptions;
using WebApi.Services.Categories;
using WebApi.Services.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ISender _sender;

    public CategoryController(ISender sender) => _sender = sender;
    
    [HttpGet]
    public async Task<IActionResult> List()
    {
        try
        {
            return Ok(await _sender.Send(new ListCategoriesRequest()));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("online-menu")]
    public async Task<IActionResult> ListOnlineMenu(string storeHostname)
    {
        try
        {
            return Ok(await _sender.Send(new ListCategoriesRequest
            {
                StoreHostname = storeHostname
            }));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
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
    public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request)
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
    [Route("Sort")]
    public async Task<IActionResult> Sort([FromBody] UpdateCategorySortRequest request)
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

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return Ok(await _sender.Send(new DeleteCategoryRequest { Id = id }));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}