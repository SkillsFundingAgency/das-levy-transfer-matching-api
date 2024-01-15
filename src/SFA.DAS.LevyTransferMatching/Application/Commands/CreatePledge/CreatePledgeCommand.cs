using System.Collections.Generic;
using MediatR;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;

public class CreatePledgeCommand : IRequest<CreatePledgeResult>
{
    public long AccountId { get; set; }
    public int Amount { get; set; }
    public string DasAccountName { get; set; }
    public bool IsNamePublic { get; set; }
    public IEnumerable<Sector> Sectors { get; set; }
    public IEnumerable<JobRole> JobRoles { get; set; }
    public IEnumerable<Level> Levels { get; set; }
    public List<LocationInformation> Locations { get; set; }
    public AutomaticApprovalOption AutomaticApprovalOption { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
}