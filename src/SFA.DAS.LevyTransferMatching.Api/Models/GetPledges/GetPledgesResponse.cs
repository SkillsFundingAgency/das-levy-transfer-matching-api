using SFA.DAS.LevyTransferMatching.Api.Models.GetPledge;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.GetPledges
{
    public class GetPledgesResponse : List<GetPledgeResponse>
    {
        public GetPledgesResponse(IEnumerable<GetPledgeResponse> collection) : base(collection)
        {
        }
    }
}