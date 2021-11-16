using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Data.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesResult : ItemListResponse<GetPledgesResult.Pledge>
    {
        public class Pledge
        {
            public int Id { get; set; }

            public long AccountId { get; set; }

            public int Amount { get; set; }

            public int RemainingAmount { get; set; }

            public int ApplicationCount { get; set; }

            public bool IsNamePublic { get; set; }

            public string DasAccountName { get; set; }

            public DateTime CreatedOn { get; set; }

            public IEnumerable<JobRole> JobRoles { get; set; }

            public IEnumerable<Level> Levels { get; set; }

            public IEnumerable<Sector> Sectors { get; set; }
            public PledgeStatus Status { get; set; }

            public IEnumerable<LocationInformation> Locations { get; set; }
        }
    }
}