using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationFundingAccepted(int applicationId, int pledgeId, bool rejectApplications) : IDomainEvent
    {
        public int ApplicationId { get; set; } = applicationId;
        public int PledgeId { get; set; } = pledgeId;
        public bool RejectApplications { get; set; } = rejectApplications;
    }
}
