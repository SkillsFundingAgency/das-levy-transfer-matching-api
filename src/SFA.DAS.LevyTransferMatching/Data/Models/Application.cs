using System;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class Application
    {
        public Application() {}

        public Application(Pledge pledge, EmployerAccount account)
        {
            Pledge = pledge;
            EmployerAccount = account;
            CreatedOn = DateTime.UtcNow;
        }

        public int Id { get; private set; }

        public EmployerAccount EmployerAccount { get; private set; }

        public Pledge Pledge { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public byte[] RowVersion { get; private set; }
    }
}
