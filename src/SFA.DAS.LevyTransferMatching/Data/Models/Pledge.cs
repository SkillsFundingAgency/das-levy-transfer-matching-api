using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    [Table(nameof(Pledge))]
    public class Pledge
    {
        public int Id { get; set; }

        public long EmployerAccountId { get; set; }

        public int Amount { get; set; }

        public bool IsNamePublic { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<PledgeLevel> PledgeLevels { get; set; }

        public IEnumerable<PledgeRole> PledgeRoles { get; set; }

        public IEnumerable<PledgeSector> PledgeSectors { get; set; }
        public byte[] RowVersion { get; set; }
    }
}