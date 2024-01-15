namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;

public class RejectApplicationCommand : IRequest
{
    public int PledgeId { get; set; }
    public int ApplicationId { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
}