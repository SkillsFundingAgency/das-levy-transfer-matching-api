using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.GenerateCostProjection
{
    public class GenerateMatchingCriteria : IRequest
    {
        public int ApplicationId { get; set; }
    }
}
