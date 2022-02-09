using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Models.GetApplication
{
    public class GetApplicationResponse
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
        public bool HasTrainingProvider { get; set; }
        public string Details { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public DateTime CreatedOn { get; set; }
        public string BusinessWebsite { get; set; }
        public string EmployerAccountName { get; set; }
        public string SenderEmployerAccountName { get; set; }
        public List<ApplicationLocation> Locations { get; set; }
        public string AdditionalLocation { get; set; }
        public string SpecificLocation { get; set; }
        public int Amount { get; set; }
        public int TotalAmount { get; set; }
        public ApplicationStatus Status { get; set; }
        public int PledgeId { get; set; }
        public long SenderEmployerAccountId { get; set; }
        public long ReceiverEmployerAccountId { get; set; }
        public string PledgeEmployerAccountName { get; set; }
        public int AmountUsed { get; set; }
        public int NumberOfApprenticesUsed { get; set; }
        public bool AutomaticApproval { get; set; }
        public IEnumerable<CostProjection> CostProjections { get; set; }

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

        public static implicit operator GetApplicationResponse(GetApplicationResult getApplicationResult)
        {
            return new GetApplicationResponse()
            {
                BusinessWebsite = getApplicationResult.BusinessWebsite,
                Details = getApplicationResult.Details,
                EmailAddresses = getApplicationResult.EmailAddresses,
                CreatedOn = getApplicationResult.CreatedOn,
                FirstName = getApplicationResult.FirstName,
                HasTrainingProvider = getApplicationResult.HasTrainingProvider,
                LastName = getApplicationResult.LastName,
                NumberOfApprentices = getApplicationResult.NumberOfApprentices,
                Sectors = getApplicationResult.Sectors,
                StandardId = getApplicationResult.StandardId,
                StandardTitle = getApplicationResult.StandardTitle,
                StandardLevel = getApplicationResult.StandardLevel,
                StandardDuration = getApplicationResult.StandardDuration,
                StandardMaxFunding = getApplicationResult.StandardMaxFunding,
                StandardRoute = getApplicationResult.StandardRoute,
                StartDate = getApplicationResult.StartDate,
                EmployerAccountName = getApplicationResult.EmployerAccountName,
                SenderEmployerAccountName = getApplicationResult.SenderEmployerAccountName,
                Locations = getApplicationResult.Locations.Select(x => new ApplicationLocation { PledgeLocationId = x.PledgeLocationId }).ToList(),
                AdditionalLocation = getApplicationResult.AdditionalLocation,
                SpecificLocation = getApplicationResult.SpecificLocation,
                Amount = getApplicationResult.Amount,
                TotalAmount = getApplicationResult.TotalAmount,
                Status = getApplicationResult.Status,
                PledgeId = getApplicationResult.PledgeId,
                SenderEmployerAccountId = getApplicationResult.SenderEmployerAccountId,
                ReceiverEmployerAccountId = getApplicationResult.ReceiverEmployerAccountId,
                PledgeEmployerAccountName = getApplicationResult.PledgeEmployerAccountName,
                AmountUsed = getApplicationResult.AmountUsed,
                NumberOfApprenticesUsed = getApplicationResult.NumberOfApprenticesUsed,
                AutomaticApproval = getApplicationResult.AutomaticApproval,
                CostProjections = getApplicationResult.CostProjections?.Select(p => new CostProjection{ FinancialYear = p.FinancialYear, Amount = p.Amount })
            };
        }
    }
}