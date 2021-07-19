namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class PledgeLocation
    {
        public int Id { get; set; }
        public int PledgeId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
