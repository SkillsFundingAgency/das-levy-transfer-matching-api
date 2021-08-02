using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class CreateApplicationResponse
    {
        public int ApplicationId { get; set; }

        public static implicit operator CreateApplicationResponse(CreateApplicationCommandResult commandResult)
        {
            return new CreateApplicationResponse()
            {
                ApplicationId = commandResult.ApplicationId
            };
        }
    }
}
