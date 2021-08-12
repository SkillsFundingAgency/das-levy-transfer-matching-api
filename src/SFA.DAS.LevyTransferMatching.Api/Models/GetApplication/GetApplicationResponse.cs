using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
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
            };
        }
    }
}