using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationCreated(int applicationId, int pledgeId, long receiverAccountId, DateTime createdOn)
    : IDomainEvent
{
    public int ApplicationId { get; } = applicationId;
    public int PledgeId { get; } = pledgeId;
    public long ReceiverAccountId { get; set; } = receiverAccountId;

    public DateTime CreatedOn { get; } = createdOn;
}