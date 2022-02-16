using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.GenerateCostProjection
{
    public class GenerateCostProjectionCommand : IRequest
    {
        public int ApplicationId { get; set; }
    }
}
