using MediatR;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class DebitApplicationRequest
    {
        public int NumberOfApprentices { get; set; }
        public int Amount { get; set; }
    }
}
