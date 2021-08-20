using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Models.GetPledges
{
    public class GetPledgesResponse
    {
        public IEnumerable<Pledge> Pledges { get; set; }
        public int TotalPledges { get; set; }

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
                    ApplicationCount = pledge.ApplicationCount
                };
            }
        }
    }
}