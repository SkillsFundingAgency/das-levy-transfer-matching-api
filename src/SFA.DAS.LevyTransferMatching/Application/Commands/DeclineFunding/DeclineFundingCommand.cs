namespace SFA.DAS.LevyTransferMatching.Application.Commands.DeclineFunding;

public class DeclineFundingCommand : IRequest<DeclineFundingCommandResult>
{
    public long? AccountId { get; set; }
    public int ApplicationId { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
}