namespace SFA.DAS.LevyTransferMatching.Models
{
    public class LocationInformation
    {
        public string Name { get; set; }
        public double[] Geopoint { get; set; }
        public string LocalAuthorityName { get; set; }
        public string LocalAuthorityDistrict { get; set; }
        public string County { get; set; }
        public string Region { get; set; }
    }
}
