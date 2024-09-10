using WebApi.Services.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Exceptions;
using WebApi.Services.Reviews;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ISender _sender;

    public ReviewController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> List(DateTime from, DateTime to ,int page = 1, int limit = 10 )
    {
        try
        {
            return Ok(await _sender.Send(new ListReviewsRequest
            {
                Page = page,
                Limit = limit,
                From = from,
                To = to
            }));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewRequest createReviewRequest)
    {
        try
        {
            return Ok(await _sender.Send(createReviewRequest));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}
