using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Models.GetPledges;

public class GetPledgesResponse
{
    public IEnumerable<Pledge> Pledges { get; set; }
    public int TotalPledges { get; set; }

    public int Page { get; set; }

    public int TotalPages { get; set; }

    public int PageSize { get; set; }

    public class Pledge
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

        public IEnumerable<LocationInformation> Locations { get; set; }

        public int ApplicationCount { get; set; }

        public static implicit operator Pledge(GetPledgesResult.Pledge pledge)
        {
            return new Pledge()
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
                Locations = pledge.Locations,
                ApplicationCount = pledge.ApplicationCount
            };
        }
    }
}