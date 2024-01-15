using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;

public class DebitApplicationCommand : IRequest
{
    public int ApplicationId { get; set; }
    public int NumberOfApprentices { get; set; }
    public int Amount { get; set; }
    public int MaxAmount { get; set; }
}