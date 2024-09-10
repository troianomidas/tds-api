using WebApi.Services.Common.Security;
using WebApi.Services.Financials;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Financials;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class FinancialController : ControllerBase
{
    private readonly ISender _sender;

    public FinancialController(ISender sender) => _sender = sender;
    
    [HttpGet]
    public async Task<IActionResult> Get(DateTime dateFilterDate)
    {
        try
        {
            return Ok(await _sender.Send(new GetFinancialsRequest
            {
                DateFilterDate = dateFilterDate
            }));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
}