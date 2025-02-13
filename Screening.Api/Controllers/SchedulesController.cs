using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Screening.Application.Feature.Management.Courses.Commands;
using Screening.Application.Feature.Management.Courses.Queries;
using Screening.Application.Feature.Management.Schedules.Commands;
using Screening.Application.Feature.Management.Schedules.Queries;
using Screening.Common.Extensions;
using Screening.Common.Models.Courses;
using Screening.Common.Models.Schedules;

namespace Screening.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SchedulesController : BaseController
{
    [HttpPost("add")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ScheduleRequest request)
    {
        var response = await Sender.Send(new CreateScheduleCommand(request));

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
        var response = await Sender.Send(new DeleteScheduleCommand() { Id = id });

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
        var response = await Sender.Send(new GetScheduleQuery(id));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List([FromQuery] DataGridQuery query, string access)
    {
        var response = await Sender.Send(new ListScheduleQuery { GridQuery = query, Access = access});

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("schoolyears")]
    [Authorize]
    public async Task<IActionResult> SchoolYears()
    {
        var response = await Sender.Send(new ListSchoolYearsQuery());

        if (response != null)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
