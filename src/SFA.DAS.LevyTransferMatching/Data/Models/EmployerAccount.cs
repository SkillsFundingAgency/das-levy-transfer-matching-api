using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class EmployerAccount : AggregateRoot
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public Pledge CreatePledge(int amount, bool isNamePublic, Level levels, JobRole jobRoles, Sector sectors, List<PledgeLocation> locations)
        {
            return new Pledge(this, amount, isNamePublic, levels, jobRoles, sectors, locations);
        }
    }
}
