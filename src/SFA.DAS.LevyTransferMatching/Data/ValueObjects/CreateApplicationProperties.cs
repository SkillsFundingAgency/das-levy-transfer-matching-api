using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.ValueObjects
{
    public class CreateApplicationProperties
    {
        public string Details { get; set; }
        public string StandardId { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public int StandardDuration { get; set; }
        public int StandardMaxFunding { get; set; }
        public string StandardRoute { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasTrainingProvider { get; set; }
        public int Amount { get; set; }
        public Sector Sectors { get; set; }
        public List<int> Locations { get; set; }
        public string AdditionalLocation { get; set; }
        public string SpecificLocation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessWebsite { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public IEnumerable<CostProjection> CostProjections { get; set; }
        public MatchingCriteria MatchingCriteria { get; set; }

        public CreateApplicationProperties()
        {
            EmailAddresses = new List<string>();
            CostProjections = new List<CostProjection>();
            MatchingCriteria = new MatchingCriteria(false, false, false, false);
        }
    }
}
