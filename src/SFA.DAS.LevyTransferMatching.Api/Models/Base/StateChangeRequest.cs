namespace SFA.DAS.LevyTransferMatching.Api.Models.Base;

public abstract class StateChangeRequest
{
    public string UserId { get; set; }

    public string UserDisplayName { get; set; }
}