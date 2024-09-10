using WebApi.Domain.Exceptions;
using WebApi.Services.Announcements;
using WebApi.Services.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]

public class AnnouncementController : ControllerBase
{
    private readonly ISender _sender;

    public AnnouncementController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> List()
    {
        try
        {
            return Ok(await _sender.Send(new ListAnnouncementsRequest()));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            return Ok(await _sender.Send(new GetAnnouncementById
            {
                AnnouncementId = id
            }));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}