using System;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationWithdrawn : IDomainEvent
{
    public ApplicationWithdrawn(int applicationId, int pledgeId, DateTime withdrawnOn)
    {
        ApplicationId = applicationId;
        WithdrawnOn = withdrawnOn;
        PledgeId = pledgeId;
    }

    public int ApplicationId { get; }
    public int PledgeId { get; }
    public DateTime WithdrawnOn { get; }
}