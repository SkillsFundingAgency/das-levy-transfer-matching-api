using System;
using SFA.DAS.LevyTransferMatching.Abstractions;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class PledgeLedger : Entity<int>
    {
        public int PledgeId { get; protected set; }
        public int? ApplicationId { get; protected set; }
        public string UserAction { get; protected set; }
        public DateTime ActionDate { get; protected set; }
        public int Amount { get; protected set; }
        public int Balance { get; protected set; }

        public PledgeLedger(int? applicationId, string userAction, int amount, int balance)
        {
            ApplicationId = applicationId;
            UserAction = userAction;
            ActionDate = DateTime.UtcNow;
            Amount = amount;
            Balance = balance;
        }
    }
}
