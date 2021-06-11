using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    [Route("tags")]
    public class TagsController : Controller
    {
        [Route("levels")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet]
        public IActionResult Levels()
        {
            return Ok(EnumExtensions.ConvertToTags<Level>());
        }

        [Route("sectors")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet]
        public IActionResult Sectors()
        {
            return Ok(EnumExtensions.ConvertToTags<Sector>());
        }

        [Route("jobRoles")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet]
        public IActionResult JobRoles()
        {
            return Ok(EnumExtensions.ConvertToTags<JobRole>());
        }
    }
}
