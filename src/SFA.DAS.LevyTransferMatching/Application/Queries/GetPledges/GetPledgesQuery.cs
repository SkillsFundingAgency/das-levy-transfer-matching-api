using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesResult>
    {
        public int? Id { get; set; }
    }
}
