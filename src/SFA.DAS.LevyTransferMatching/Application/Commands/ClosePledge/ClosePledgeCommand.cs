using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge
{
    public class ClosePledgeCommand : IRequest<ClosePledgeResult>
    {
        public int PledgeId { get; set; }

    }
}
