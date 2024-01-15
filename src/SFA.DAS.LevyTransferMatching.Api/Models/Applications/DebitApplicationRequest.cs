namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications;

public class DebitApplicationRequest
{
    public int NumberOfApprentices { get; set; }
    public int Amount { get; set; }
    public int MaxAmount { get; set; }
}