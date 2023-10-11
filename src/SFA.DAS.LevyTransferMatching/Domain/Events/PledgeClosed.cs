using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class PledgeClosed : IDomainEvent
    {
        public PledgeClosed(int pledgeId, bool insufficientFunds)
        {
            PledgeId = pledgeId;
            InsufficientFunds = insufficientFunds;
        }

        public int PledgeId { get; set; }
        public bool InsufficientFunds { get; set; }
    }
}