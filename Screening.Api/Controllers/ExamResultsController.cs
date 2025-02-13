using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Screening.Application.Feature.Management.AssessmentsResult.Exams.Commands;
using Screening.Application.Feature.Management.AssessmentsResult.Exams.Queries;
using Screening.Common.Models.Examinations;

namespace Screening.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExamResultsController : BaseController
{
    [HttpPost("score")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ExaminationResultRequest request)
    {
        var response = await Sender.Send(new CreateExamResultCommand(request));

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
        var response = await Sender.Send(new GetExamResultQuery(id));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] ExaminationResultUpdate update)
    {
        var response = await Sender.Send(new UpdateExamResultCommand(update));

        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
