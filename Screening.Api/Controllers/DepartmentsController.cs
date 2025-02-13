using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Screening.Application.Feature.Management.Campuses.Commands;
using Screening.Application.Feature.Management.Campuses.Queries;
using Screening.Application.Feature.Management.Departments.Commands;
using Screening.Application.Feature.Management.Departments.Queries;
using Screening.Common.Extensions;
using Screening.Common.Models.Campuses;
using Screening.Common.Models.Departments;

namespace Screening.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : BaseController
{
    [HttpPost("add")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] DepartmentRequest request)
    {
        var response = await Sender.Send(new CreateDepartmentCommand(request));

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
        var response = await Sender.Send(new DeleteDepartmentCommand() { Id = id });

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
        var response = await Sender.Send(new GetDepartmentQuery(id));

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
        var response = await Sender.Send(new ListDepartmentQuery { GridQuery = query });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateAccess([FromBody] DepartmentUpdate update)
    {
        var response = await Sender.Send(new UpdateDepartmentCommand(update));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
