using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Screening.Common.Models.Auth;
using Screening.Infrastructure.Identity.Features.Auths.Queries;

namespace Screening.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
	
    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthRequest request)
    {        
        var response = await Sender.Send(new AuthQuery { Get = request });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {        
        var response = await Sender.Send(new RefreshTokenQuery { Get = request });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost("remove-refresh-token")]
    
    public async Task<IActionResult> RemoveRefreshToken([FromBody] RefreshTokenRequest request)
    {       
        var response = await Sender.Send(new RemoveRefreshTokenQuery { Get = request });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("current-user")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
      
        var response = await Sender.Send(new CurrentUserQuery { });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
