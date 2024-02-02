using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("reference")]
public class ReferenceController : Controller
{
    [Route("levels")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [HttpGet]
    public IActionResult Levels()
    {
        return Ok(EnumExtensions.ConvertToReferenceData<Level>());
    }

    [Route("sectors")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [HttpGet]
    public IActionResult Sectors()
    {
        return Ok(EnumExtensions.ConvertToReferenceData<Sector>());
    }

    [Route("jobRoles")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [HttpGet]
    public IActionResult JobRoles()
    {
        return Ok(EnumExtensions.ConvertToReferenceData<JobRole>());
    }
}