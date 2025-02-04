namespace SFA.DAS.LevyTransferMatching.Application.Commands.ExpireAcceptedFunding;

public class ExpireAcceptedFundingCommand : IRequest<ExpireAcceptedFundingCommandResult>
{   
    public int ApplicationId { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
}
