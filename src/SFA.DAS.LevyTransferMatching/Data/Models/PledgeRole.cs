using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    [Table(nameof(PledgeRole))]
    public class PledgeRole
    {
        public int PledgeRoleId { get; set; }

        public int PledgeId { get; set; }

        public byte RoleId { get; set; }
    }
}