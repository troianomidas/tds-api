using WebApi.Domain.Exceptions;
using WebApi.Services.Common.Security;
using WebApi.Services.Customers;
using WebApi.Services.Rewards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class RewardController : ControllerBase
{
    private readonly ISender _sender;

    public RewardController(ISender sender) => _sender = sender;

    [HttpGet]
    [Route("customer")]
    public async Task<IActionResult> List(int? status, int page = 1, int limit = 10)
    {
        try
        {
            return Ok(await _sender.Send(new ListRewardsRequest
            {
                Status = status
            }));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("store")]
    public async Task<IActionResult> List(int? id)
    {
        try
        {
            return Ok(await _sender.Send(new ListStoreRewardsRequest
            {
                Id = id
            }));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("transaction")]
    public async Task<IActionResult> List(int storeId, int page = 1, int limit = 5)
    {
        try
        {
            return Ok(await _sender.Send(new ListRewardTransactionsRequest
            {
                StoreId = storeId,
                Page = page,
                Limit = limit
            }));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("customer-balance")]
    public async Task<IActionResult> List(int storeId, string? filter)
    {
        try
        {
            return Ok(await _sender.Send(new ListCustomerBalanceRequest
            {
                StoreId = storeId,
                Filter = filter
            }));
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRewardRequest createRewardRequest)
    {
        try
        {
            return Ok(await _sender.Send(createRewardRequest));
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

    [HttpPost]
    [Route("transaction")]
    public async Task<IActionResult> Create([FromBody] CreateRewardTransactionRequest createRewardTransactionRequest)
    {
        try
        {
            return Ok(await _sender.Send(createRewardTransactionRequest));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateRewardRequest updateRewardRequest)
    {
        try
        {
            return Ok(await _sender.Send(updateRewardRequest));
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

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteRewardRequest deleteRewardRequest)
    {
        try
        {
            return Ok(await _sender.Send(deleteRewardRequest));
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