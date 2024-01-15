using SFA.DAS.LevyTransferMatching.Api.Models.Base;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications;

public class ApproveApplicationRequest : StateChangeRequest
{
    public bool AutomaticApproval { get; set; }
}