using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

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

        public Pledge CreatePledge(CreatePledgeProperties properties, UserInfo userInfo)
        {
            return new Pledge(this, properties, userInfo);
        }
    }
}
