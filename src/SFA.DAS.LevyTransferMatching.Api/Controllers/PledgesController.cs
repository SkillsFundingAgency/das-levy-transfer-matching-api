﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Models;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [Route("accounts/{EncodedAccountId}/pledges")]
    [ApiController]
    public class PledgesController
    {
        private readonly IMediator _mediator;

        public PledgesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(string encodedAccountId, [FromBody]CreatePledgeRequest request)
        {
            var commandResult = await _mediator.Send(new CreatePledgeCommand()
            {
                Pledge = new Pledge()
                {
                    EncodedAccountId = encodedAccountId,
                    Amount = request.Amount,
                    IsNamePublic = request.IsNamePublic,
                    JobRoles = request.JobRoles,
                    Levels = request.Levels,
                    Sectors = request.Sectors,
                }
            });

            return new AcceptedResult(
                $"/accounts/{encodedAccountId}/pledges/{commandResult.Id}",
                new PledgeDto()
                {
                    Id = commandResult.Id,
                });
        }
    }
}