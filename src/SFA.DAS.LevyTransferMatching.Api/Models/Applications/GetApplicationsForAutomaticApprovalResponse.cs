using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsForAutomaticApproval;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications;

public class GetApplicationsForAutomaticApprovalResponse
{
    public IEnumerable<Application> Applications { get; set; }

    public static implicit operator GetApplicationsForAutomaticApprovalResponse(GetApplicationsForAutomaticApprovalResult source)
    {
        return new GetApplicationsForAutomaticApprovalResponse
        {
            Applications = source.Applications.Select(application => (Application)application).ToList()
        };
    }

    public class Application
    {
        public int Id { get; set; }
        public long EmployerAccountId { get; set; }
        public int PledgeId { get; set; }
        public int MatchPercentage { get; set; }
        public AutomaticApprovalOption PledgeAutomaticApprovalOption { get; set; }
        public int Amount { get; set; }
        public int PledgeRemainingAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }

        public static implicit operator Application(GetApplicationsForAutomaticApprovalResult.Application source)
        {
            return new Application
            {
                Id = source.Id,
                EmployerAccountId = source.EmployerAccountId,
                PledgeId = source.PledgeId,
                MatchPercentage = source.MatchPercentage,
                PledgeAutomaticApprovalOption = source.PledgeAutomaticApprovalOption,
                Amount = source.Amount,
                PledgeRemainingAmount = source.PledgeRemainingAmount,
                CreatedOn = source.CreatedOn,
                Status = source.Status
            };
        }
    }
}
