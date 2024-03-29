﻿using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationRejected : IDomainEvent
{
    public ApplicationRejected(int applicationId, int pledgeId, DateTime rejectedOn, int amount, long receiverAccountId)
    {
        Amount = amount;
        ApplicationId = applicationId;
        RejectedOn = rejectedOn;
        PledgeId = pledgeId;
        ReceiverAccountId = receiverAccountId;
    }

    public int ApplicationId { get; }
    public int PledgeId { get; }
    public DateTime RejectedOn { get; }
    public int Amount { get; }
    public long ReceiverAccountId { get; set; }
}