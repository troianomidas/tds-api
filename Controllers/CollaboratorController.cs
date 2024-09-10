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
public class CollaboratorController : ControllerBase
{
    private readonly ISender _sender;

    public CollaboratorController(ISender sender) => _sender = sender;
    
    [HttpGet]
    public async Task<IActionResult> List(int status, string? groupName)
    {
        try
        {
            return Ok(await _sender.Send(new ListCollaboratorRequest
            {
                Status = status,
                GroupName = groupName
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCollaboratorRequest request)
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
    public async Task<IActionResult> Edit(EditCollaboratorRequest request)
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
    public async Task<IActionResult> Delete(DeleteCollaboratorRequest request)
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