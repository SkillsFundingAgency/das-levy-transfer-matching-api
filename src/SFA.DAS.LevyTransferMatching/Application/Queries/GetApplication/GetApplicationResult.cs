using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;

public class GetApplicationResult
{
    public string StandardId { get; set; }
    public string StandardTitle { get; set; }
    public int StandardLevel { get; set; }
    public int StandardDuration { get; set; }
    public int StandardMaxFunding { get; set; }
    public string StandardRoute { get; set; }
    public IEnumerable<Sector> Sectors { get; set; }
    public int NumberOfApprentices { get; set; }
    public DateTime StartDate { get; set; }
    public int Amount { get; set; }
    public int TotalAmount { get; set; }
    public bool HasTrainingProvider { get; set; }
    public string Details { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IEnumerable<string> EmailAddresses { get; set; }
    public DateTime CreatedOn { get; set; }
    public string BusinessWebsite { get; set; }
    public string EmployerAccountName { get; set; }
    public string SenderEmployerAccountName { get; set; }
    public int AmountUsed { get; set; }
    public int NumberOfApprenticesUsed { get; set; }
    public string PledgeEmployerAccountName { get; set; }
    public List<ApplicationLocation> Locations { get; set; }
    public string AdditionalLocation { get; set; }
    public string SpecificLocation { get; set; }
    public ApplicationStatus Status { get; set; }
    public int PledgeId { get; set; }
    public long ReceiverEmployerAccountId { get; set; }
    public long SenderEmployerAccountId { get; set; }
    public bool AutomaticApproval { get; set; }
    public IEnumerable<CostProjection> CostProjections { get; set; }
    public bool MatchSector { get; set; }
    public bool MatchJobRole { get; set; }
    public bool MatchLevel { get; set; }
    public bool MatchLocation { get; set; }
    public int MatchPercentage { get; set; }
    public ApplicationCostingModel CostingModel { get; set; }
    public int PledgeRemainingAmount { get; set; }
    public AutomaticApprovalOption AutomaticApprovalOption { get; set; }

    public class ApplicationLocation
    {
        public int Id { get; set; }
        public int PledgeLocationId { get; set; }
    }

    public class CostProjection
    {
        public string FinancialYear { get; set; }
        public int Amount { get; set; }
    }
}