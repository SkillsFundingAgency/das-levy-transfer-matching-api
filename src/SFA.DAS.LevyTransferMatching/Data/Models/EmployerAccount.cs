using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class EmployerAccount : AggregateRoot<long>
    {
        public string Name { get; set; }

        public static EmployerAccount New(long id, string name)
        {
            return new EmployerAccount(id, name);
        }

        private EmployerAccount(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public Pledge CreatePledge(int amount, bool isNamePublic, Level levels, JobRole jobRoles, Sector sectors, List<PledgeLocation> locations)
        {
            return new Pledge(this, amount, isNamePublic, levels, jobRoles, sectors, locations);
        }
    }
}
