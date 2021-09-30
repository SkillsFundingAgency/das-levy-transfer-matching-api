using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Models.GetApplication
{
    public class GetApplicationResponse
    {
        public string Postcode { get; set; }
        public string StandardId { get; set; }
        public IEnumerable<Sector> Sectors { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasTrainingProvider { get; set; }
        public string Details { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
        public string EmployerAccountName { get; set; }
        public List<string> PledgeLocations { get; set; }
        public IEnumerable<Sector> PledgeSectors { get; set; }
        public IEnumerable<Level> PledgeLevels { get; set; }
        public IEnumerable<JobRole> PledgeJobRoles { get; set; }
        public int PledgeRemainingAmount { get; set; }
        public int Amount { get; set; }
        public ApplicationStatus Status { get; set; }
        public bool PledgeIsNamePublic { get; set; }
        public int PledgeId { get; set; }
        public long SenderEmployerAccountId { get; set; }
        public long ReceiverEmployerAccountId { get; set; }

        public static implicit operator GetApplicationResponse(GetApplicationResult getApplicationResult)
        {
            return new GetApplicationResponse()
            {
                BusinessWebsite = getApplicationResult.BusinessWebsite,
                Details = getApplicationResult.Details,
                EmailAddresses = getApplicationResult.EmailAddresses,
                FirstName = getApplicationResult.FirstName,
                HasTrainingProvider = getApplicationResult.HasTrainingProvider,
                LastName = getApplicationResult.LastName,
                NumberOfApprentices = getApplicationResult.NumberOfApprentices,
                Postcode = getApplicationResult.Postcode,
                Sectors = getApplicationResult.Sectors,
                StandardId = getApplicationResult.StandardId,
                StartDate = getApplicationResult.StartDate,
                EmployerAccountName = getApplicationResult.EmployerAccountName,
                PledgeLocations = getApplicationResult.PledgeLocations,
                PledgeSectors = getApplicationResult.PledgeSectors,
                PledgeLevels = getApplicationResult.PledgeLevels,
                PledgeJobRoles = getApplicationResult.PledgeJobRoles,
                PledgeRemainingAmount = getApplicationResult.PledgeRemainingAmount,
                Amount = getApplicationResult.Amount,
                Status = getApplicationResult.Status,
                PledgeIsNamePublic = getApplicationResult.PledgeIsNamePublic,
                PledgeId = getApplicationResult.PledgeId,
                SenderEmployerAccountId = getApplicationResult.SenderEmployerAccountId,
                ReceiverEmployerAccountId = getApplicationResult.ReceiverEmployerAccountId,
            };
        }
    }
}