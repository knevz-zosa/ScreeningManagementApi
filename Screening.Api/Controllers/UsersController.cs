using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Screening.Common.Extensions;
using Screening.Common.Models.Users;
using Screening.Infrastructure.Identity.Features.Users.Commands;
using Screening.Infrastructure.Identity.Features.Users.Queries;

namespace Screening.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : BaseController
{    
    // POST api/user/register
    [HttpPost("register")]
    public async Task<IActionResult> Create([FromBody] UserRequest request)
    {        
        var response = await Sender.Send(new CreateUserCommand { User = request });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);

    }

    // DELETE api/user/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {      
        var response = await Sender.Send(new DeleteUserCommand { Id = id });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    // GET api/user/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {        
        var response = await Sender.Send(new GetUserQuery { Id = id });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    // GET api/user/list  

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List([FromQuery] DataGridQuery query)
    {        
        var response = await Sender.Send(new ListUserQuery { GridQuery = query });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    // GET api/user/roles
    [HttpGet("roles")]
    [Authorize]
    public async Task<IActionResult> Roles([FromQuery] DataGridQuery query)
    {
        var response = await Sender.Send(new ListRoleQuery { GridQuery = query });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    // PUT api/user/update-access
    [HttpPut("update-access")]
    [Authorize]
    public async Task<IActionResult> UpdateAccess([FromBody] UserAccessUpdate update)
    {       
        var response = await Sender.Send(new UpdateUserAccessCommand { User = update });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    // PUT api/user/update-password
    [HttpPut("update-password")]
    [Authorize]
    public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordUpdate update)
    {
        var response = await Sender.Send(new UpdateUserPasswordCommand { User = update });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    // PUT api/user/update-profile
    [HttpPut("update-profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdate update)
    {       
        var response = await Sender.Send(new UpdateUserProfileCommand { User = update });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    // PUT api/user/update-username
    [HttpPut("update-username")]
    [Authorize]
    public async Task<IActionResult> UpdateUsername([FromBody] UserUsernameUpdate update)
    {       
        var response = await Sender.Send(new UpdateUserUsernameCommand { User = update });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    // PUT api/user/update-role
    [HttpPut("update-role")]
    [Authorize]
    public async Task<IActionResult> UpdateRole([FromBody] UserRoleUpdate update)
    {
        var response = await Sender.Send(new UpdateUserRoleCommand { User = update });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    // PUT api/user/update-status
    [HttpPut("update-status")]
    [Authorize]
    public async Task<IActionResult> Status([FromBody] UserStatusUpdate update)
    {       
        var response = await Sender.Send(new UpdateUserStatusCommand { User = update });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
