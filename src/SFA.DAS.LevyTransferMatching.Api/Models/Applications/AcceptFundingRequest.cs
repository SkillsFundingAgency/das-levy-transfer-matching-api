﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Api.Models.Base;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class AcceptFundingRequest : StateChangeRequest
    {
        public int ApplicationId { get; set; }
        public long AccountId { get; set; }
    }
}
