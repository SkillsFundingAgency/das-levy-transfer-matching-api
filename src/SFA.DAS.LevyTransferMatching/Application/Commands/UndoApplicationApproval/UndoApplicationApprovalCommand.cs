namespace SFA.DAS.LevyTransferMatching.Application.Commands.UndoApplicationApproval;

public class UndoApplicationApprovalCommand : IRequest
{
    public int PledgeId { get; set; }
    public int ApplicationId { get; set; }
}