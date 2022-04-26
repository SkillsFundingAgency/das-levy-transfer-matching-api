using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplicationAfterAcceptance
{
    public class WithdrawApplicationAfterAcceptanceCommand : IRequest
    {
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}
