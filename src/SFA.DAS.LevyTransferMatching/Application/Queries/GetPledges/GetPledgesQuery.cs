using MediatR;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesQuery : PagedQuery, IRequest<GetPledgesResult>
    {
        public long? AccountId { get; set; }
        public IEnumerable<Sector> Sectors { get; set; }
    }
}