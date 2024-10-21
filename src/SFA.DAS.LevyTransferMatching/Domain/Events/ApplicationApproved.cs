using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationApproved(int applicationId, int pledgeId, DateTime approvedOn, int amount, long receiverAccountId)
    : IDomainEvent
{
    public int ApplicationId { get; } = applicationId;
    public int PledgeId { get; } = pledgeId;
    public DateTime ApprovedOn { get; } = approvedOn;
    public int Amount { get; } = amount;
    public long ReceiverAccountId { get; set; } = receiverAccountId;
}