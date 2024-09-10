using WebApi.Services.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Exceptions;
using WebApi.Services.Common.Models;

namespace WebApi.Controllers;

[AllowAnonymous]
[ApiController]
[Route("v1/[controller]")]

public class PingController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Ping()
    {
        return Ok(new
        {
            Version = "v0.10.1",
            CurrentTime = DateTimeUtils.Now(),
            CurrentTimeUtc = DateTimeUtils.Now()
        });
    }
}