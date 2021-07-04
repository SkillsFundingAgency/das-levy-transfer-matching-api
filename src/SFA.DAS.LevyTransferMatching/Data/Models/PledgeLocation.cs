using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    [Table(nameof(PledgeLocation))]
    public class PledgeLocation
    {
        public int Id { get; set; }
        public long PledgeId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
