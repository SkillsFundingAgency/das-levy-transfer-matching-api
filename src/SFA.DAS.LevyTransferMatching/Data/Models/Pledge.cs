using SFA.DAS.LevyTransferMatching.Models.Enums;
using System;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class Pledge
    {
        protected Pledge() {}

        public Pledge(EmployerAccount employerAccount, int amount, bool isNamePublic, Level levels, JobRole jobRoles, Sector sectors)
        {
            EmployerAccount = employerAccount;
            Amount = amount;
            IsNamePublic = isNamePublic;
            Levels = levels;
            JobRoles = jobRoles;
            Sectors = sectors;
        }

        public int Id { get; private set; }

        public EmployerAccount EmployerAccount { get; private set; }

        public int Amount { get; private set; }

        public bool IsNamePublic { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public JobRole JobRoles { get; private set; }
        
        public Level Levels { get; private set; }

        public Sector Sectors { get; private set; }

        public byte[] RowVersion { get; private set; }
    }
}