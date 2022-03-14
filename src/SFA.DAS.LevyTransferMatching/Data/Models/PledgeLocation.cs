using SFA.DAS.LevyTransferMatching.Abstractions;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class PledgeLocation : Entity<int>
    {
        public int PledgeId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LocalAuthorityName { get; set; }
        public string LocalAuthorityDistrict { get; set; }
        public string County { get; set; }
        public string Region { get; set; }
    }
}
