using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;
using SFA.DAS.LevyTransferMatching.Data.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Models.GetPledge;

public class GetPledgeResponse
{
    public int Id { get; set; }

    public long AccountId { get; set; }

    public int Amount { get; set; }
    public int RemainingAmount { get; set; }

    public bool IsNamePublic { get; set; }

    public string DasAccountName { get; set; }

    public DateTime CreatedOn { get; set; }

    public IEnumerable<JobRole> JobRoles { get; set; }

    public IEnumerable<Level> Levels { get; set; }

    public IEnumerable<Sector> Sectors { get; set; }
    public PledgeStatus Status { get; set; }

    public IEnumerable<Location> Locations { get; set; }

    public static implicit operator GetPledgeResponse(GetPledgeResult pledge)
    {
        return new GetPledgeResponse
        {
            AccountId = pledge.AccountId,
            Amount = pledge.Amount,
            RemainingAmount = pledge.RemainingAmount,
            CreatedOn = pledge.CreatedOn,
            DasAccountName = pledge.DasAccountName,
            Id = pledge.Id,
            IsNamePublic = pledge.IsNamePublic,
            JobRoles = pledge.JobRoles,
            Levels = pledge.Levels,
            Sectors = pledge.Sectors,
            Status = pledge.Status,
            Locations = pledge.Locations.Select(x => new Location { Id = x.Id, Name = x.Name })
        };
    }

    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}