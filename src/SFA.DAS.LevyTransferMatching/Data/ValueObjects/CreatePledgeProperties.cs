using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.ValueObjects;

public class CreatePledgeProperties
{
    public int Amount { get; set; }
    public bool IsNamePublic { get; set; }
    public AutomaticApprovalOption AutomaticApprovalOption { get; set; }
    public Level Levels { get; set; }
    public JobRole JobRoles { get; set; }
    public Sector Sectors { get; set; }
    public List<PledgeLocation> Locations { get; set; }

    public CreatePledgeProperties()
    {
        Locations = new List<PledgeLocation>();
    }
}