using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Screening.Application.Feature.Management.Applicants.Commands;
using Screening.Application.Feature.Management.Applicants.Queries;
using Screening.Application.Feature.Management.Departments.Queries;
using Screening.Common.Extensions;
using Screening.Common.Models.Academics;
using Screening.Common.Models.Applicants;
using Screening.Common.Models.EmergencyContacts;
using Screening.Common.Models.PersonalInformations;
using Screening.Common.Models.Transfers;

namespace Screening.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicantsController : BaseController
{
    [HttpPost("transfer")]
    [Authorize]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
    {
        var response = await Sender.Send(new CreateTransferCommand(request));

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
        var response = await Sender.Send(new DeleteApplicantCommand() { Id = id });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update-emergency-contact")]
    [Authorize]
    public async Task<IActionResult> UpdateEmergencyContact([FromBody] EmergencyContactUpdate update)
    {
        var response = await Sender.Send(new UpdateEmergencyContactCommand(update));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update-gwa-status-track")]
    [Authorize]
    public async Task<IActionResult> UpdateGwaStatusTrack([FromBody] ApplicantUpdateGwaStatusTrack update)
    {
        var response = await Sender.Send(new UpdateGWAStatusTrackCommand(update));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update-lrn")]
    [Authorize]
    public async Task<IActionResult> UpdateLRN([FromBody] LrnUpdate update)
    {
        var response = await Sender.Send(new UpdateLrnCommand(update));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update-personal-information")]
    [Authorize]
    public async Task<IActionResult> UpdatePersonalInformation([FromBody] PersonalInformationUpdate update)
    {
        var response = await Sender.Send(new UpdatePersonalInformationCommand(update));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update-studentid")]
    [Authorize]
    public async Task<IActionResult> UpdateStudentId([FromBody] ApplicantUpdateStudentId update)
    {
        var response = await Sender.Send(new UpdateStudentIdCommand(update));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update-transfer")]
    [Authorize]
    public async Task<IActionResult> UpdateTransfer([FromBody] ApplicantTransfer update)
    {
        var response = await Sender.Send(new UpdateTransferCommand(update));

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
        var response = await Sender.Send(new GetApplicantQuery(id));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("first-application/{id}")]
    [Authorize]
    public async Task<IActionResult> GetFirstApplication(int id)
    {
        var response = await Sender.Send(new GetFirstApplicationInfoQuery(id));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("personal-information/{id}")]
    [Authorize]
    public async Task<IActionResult> GetPersonalInformation(int id)
    {
        var response = await Sender.Send(new GetPersonalInformationQuery(id));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("lrn/{id}")]
    [Authorize]
    public async Task<IActionResult> GetLRN(int id)
    {
        var response = await Sender.Send(new GetLrnQuery(id));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List([FromQuery] DataGridQuery query, int? id, string access)
    {
        var response = await Sender.Send(new ListApplicantQuery { GridQuery = query, ScheduleId = id, Access = access });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("in-progress")]
    [Authorize]
    public async Task<IActionResult> InProgress([FromQuery] DataGridQuery query, string access)
    {
        var response = await Sender.Send(new ListInProgressQuery { GridQuery = query, Access = access });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("passers")]
    [Authorize]
    public async Task<IActionResult> Passers([FromQuery] DataGridQuery query, string access)
    {
        var response = await Sender.Send(new ListPassersQuery { GridQuery = query, Access = access });

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
