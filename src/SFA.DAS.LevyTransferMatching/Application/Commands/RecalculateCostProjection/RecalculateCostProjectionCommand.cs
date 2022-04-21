using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RecalculateCostProjection
{
    public class RecalculateCostProjectionCommand : IRequest
    {
        public int ApplicationId { get; set; }
    }
}
