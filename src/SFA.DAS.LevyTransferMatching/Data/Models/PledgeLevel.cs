using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    [Table(nameof(PledgeLevel))]
    public class PledgeLevel
    {
        public int PledgeLevelId { get; set; }

        public int PledgeId { get; set; }

        public byte LevelId { get; set; }
    }
}