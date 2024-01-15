using SFA.DAS.LevyTransferMatching.Abstractions;

namespace SFA.DAS.LevyTransferMatching.Data.Models;

public class ApplicationLocation : Entity<int>
{
    public ApplicationLocation(int pledgeLocationId)
    {
        PledgeLocationId = pledgeLocationId;
    }

    public int PledgeLocationId { get; set; }
}