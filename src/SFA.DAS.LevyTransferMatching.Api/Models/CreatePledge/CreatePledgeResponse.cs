using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;

namespace SFA.DAS.LevyTransferMatching.Api.Models.CreatePledge;

public class CreatePledgeResponse
{
    public int Id { get; set; }

    public static implicit operator CreatePledgeResponse(CreatePledgeResult createPledgeResult)
    {
        return new CreatePledgeResponse()
        {
            Id = createPledgeResult.Id,
        };
    }
}