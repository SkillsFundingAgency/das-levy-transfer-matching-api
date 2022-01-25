using MediatR;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class DebitPledgeCommand : IRequest<DebitPledgeCommandResult>
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public int Amount { get; set; }
        public UserAction UserAction { get; set; }
    }
}
