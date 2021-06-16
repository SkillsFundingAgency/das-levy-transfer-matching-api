using SFA.DAS.LevyTransferMatching.Models;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries
{
    public class GetAllPledgesResult
    {
        public IEnumerable<Pledge> Pledges { get; set; }
    }
}
