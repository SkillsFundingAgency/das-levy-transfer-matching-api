using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    [Table(nameof(EmployerAccount))]
    public class EmployerAccount
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
