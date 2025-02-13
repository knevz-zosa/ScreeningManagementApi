using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Screening.Api.Controllers;
[ApiController]
public class BaseController : ControllerBase
{
    private ISender _sender = null;

    public ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
