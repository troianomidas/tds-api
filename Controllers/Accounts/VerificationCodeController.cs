// using WebApi.Domain.Constants;
// using WebApi.Domain.Exceptions;
// using WebApi.Integrations.Queues;
// using WebApi.Services.Subscriptions;
// using MediatR;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using WebApi.Services.Accounts;
// using WebApi.Services.ReferralCodes;
// // using WebApi.Services.VerificationCodes;
//
// namespace WebApi.Controllers.Accounts;
//
// [AllowAnonymous]
// [ApiController]
// [Route("v1/[controller]/[action]")]
// public class VerificationCodeController : ControllerBase
// {
//     private readonly ISender _sender;
//     
//     public VerificationCodeController(ISender sender)
//     {
//         _sender = sender;
//     }
//     
//     [HttpPost]
//     public async Task<IActionResult> Send([FromBody] SendVerificationCode request)
//     {
//         try
//         {
//             return Ok(await _sender.Send(request));
//         }
//         catch (Exception e)
//         {
//             return BadRequest(e.Message);
//         }
//     }
//     
//     [HttpPost]
//     public async Task<IActionResult> Check([FromBody] CheckVerificationCode request)
//     {
//         try
//         {
//             return Ok(await _sender.Send(request));
//         }
//         catch (Exception e)
//         {
//             return BadRequest(e.Message);
//         }
//     }
// }