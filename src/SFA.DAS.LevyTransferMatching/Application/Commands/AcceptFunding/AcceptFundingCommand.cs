using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;

public class AcceptFundingCommand : IRequest<AcceptFundingCommandResult>
{
    public long AccountId { get; set; }
    public int ApplicationId { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
}