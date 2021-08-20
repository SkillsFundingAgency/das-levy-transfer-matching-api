﻿using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesResult>
    {
        public long? AccountId { get; set; }
    }
}