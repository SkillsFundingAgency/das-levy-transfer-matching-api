using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationRejected(int applicationId, int pledgeId, DateTime rejectedOn, int amount, long receiverAccountId)
    : IDomainEvent
{
    public int ApplicationId { get; } = applicationId;
    public int PledgeId { get; } = pledgeId;
    public DateTime RejectedOn { get; } = rejectedOn;
    public int Amount { get; } = amount;
    public long ReceiverAccountId { get; set; } = receiverAccountId;
}