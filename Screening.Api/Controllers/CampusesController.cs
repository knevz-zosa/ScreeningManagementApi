using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Screening.Application.Feature.Management.Campuses.Commands;
using Screening.Application.Feature.Management.Campuses.Queries;
using Screening.Common.Extensions;
using Screening.Common.Models.Campuses;
using Screening.Common.Models.Users;
using Screening.Infrastructure.Identity.Features.Users.Commands;
using Screening.Infrastructure.Identity.Features.Users.Queries;

namespace Screening.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CampusesController : BaseController
{
    [HttpPost("add")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CampusRequest request)
    {
        var response = await Sender.Send(new CreateCampusCommand(request));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);

    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await Sender.Send(new DeleteCampusCommand() { Id = id});

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var response = await Sender.Send(new GetCampusQuery(id));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List([FromQuery] DataGridQuery query)
    {
        var response = await Sender.Send(new ListCampusQuery { GridQuery = query });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateAccess([FromBody] CampusUpdate update)
    {
        var response = await Sender.Send(new UpdateCampusCommand (update));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

}
