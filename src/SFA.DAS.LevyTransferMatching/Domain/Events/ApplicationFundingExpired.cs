using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationFundingExpired(int applicationId)
    : IDomainEvent
{
    public int ApplicationId { get; } = applicationId;  
}