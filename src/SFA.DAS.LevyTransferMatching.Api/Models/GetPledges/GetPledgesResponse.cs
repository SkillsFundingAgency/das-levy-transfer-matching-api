using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.GetPledges
{
    public class GetPledgesResponse : List<GetPledgesResponse.Pledge>
    {
        public GetPledgesResponse(IEnumerable<Pledge> collection) : base(collection)
        {
        }

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

            public IEnumerable<LocationInformation> Locations { get; set; }

            public static implicit operator Pledge(LevyTransferMatching.Models.Pledge pledge)
            {
                return new Pledge()
                {
                    AccountId = pledge.AccountId,
                    Amount = pledge.Amount,
                    RemainingAmount = pledge.Amount,
                    CreatedOn = pledge.CreatedOn,
                    DasAccountName = pledge.DasAccountName,
                    Id = pledge.Id,
                    IsNamePublic = pledge.IsNamePublic,
                    JobRoles = pledge.JobRoles,
                    Levels = pledge.Levels,
                    Sectors = pledge.Sectors,
                    Locations = pledge.Locations
                };
            }
        }
    }
}