using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge
{
    public class ClosePledgeCommand : IRequest<ClosePledgeResult>
    {
        public int PledgeId { get; set; }
    }
}
