using MediatR;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesQuery : PagedQuery, IRequest<GetPledgesResult>
    {
        public long? AccountId { get; set; }
    }
}