﻿using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge
{
    public class ClosePledgeCommand : IRequest
    {
        public int PledgeId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }

    }
}
