using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class PledgeCredited :IDomainEvent
    {
        public PledgeCredited(int pledgeId)
        {
            PledgeId = pledgeId;
        }

        public int PledgeId { get; set; }
    }
}
