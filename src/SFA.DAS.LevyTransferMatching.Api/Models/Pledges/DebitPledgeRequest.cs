using SFA.DAS.LevyTransferMatching.Api.Models.Base;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class DebitPledgeRequest : StateChangeRequest
    {
        public int Amount { get; set; }
        public int ApplicationId { get; set; }
        public UserAction UserAction { get; set; }
        
    }
}
