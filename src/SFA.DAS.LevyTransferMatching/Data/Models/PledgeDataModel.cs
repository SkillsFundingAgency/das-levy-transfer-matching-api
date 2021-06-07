using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    [Table("Pledge")]
    public class PledgeDataModel
    {
        [Key]
        public int PledgeId { get; set; }

        [StringLength(100)]
        public string EncodedId { get; set; }
        
        public long EmployerAccountId { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal Amount { get; set; }

        public bool HideEmployerName { get; set; }

        public DateTime CreationDate { get; set; }

    }
}