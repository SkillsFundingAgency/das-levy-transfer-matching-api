namespace SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;

public class WithdrawApplicationCommand : IRequest
{
    public long AccountId { get; set; }
    public int ApplicationId { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
}