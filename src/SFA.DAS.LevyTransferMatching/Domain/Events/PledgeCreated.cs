using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class PledgeCreated : IDomainEvent
    {
        public Pledge Pledge { get; }

        public PledgeCreated(Pledge pledge)
        {
            Pledge = pledge;
        }
    }
}
