using SFA.DAS.LevyTransferMatching.Abstractions;

namespace SFA.DAS.LevyTransferMatching.Data.Models;

public class ApplicationLocation(int pledgeLocationId) : Entity<int>
{
    public int PledgeLocationId { get; set; } = pledgeLocationId;
}