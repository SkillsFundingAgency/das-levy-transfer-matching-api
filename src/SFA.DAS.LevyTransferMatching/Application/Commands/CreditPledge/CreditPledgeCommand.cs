using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;

public class CreditPledgeCommand : IRequest
{
    public int PledgeId { get; set; }
    public int ApplicationId { get; set; }
    public int Amount { get; set; }
}