using SFA.DAS.LevyTransferMatching.Models.Enums;
using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Abstractions;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class Pledge : AggregateRoot<int>
    {
        protected Pledge() {}

        public Pledge(EmployerAccount employerAccount, int amount, bool isNamePublic, Level levels, JobRole jobRoles, Sector sectors, List<PledgeLocation> locations)
        {
            EmployerAccount = employerAccount;
            Amount = amount;
            IsNamePublic = isNamePublic;
            Levels = levels;
            JobRoles = jobRoles;
            Sectors = sectors;
            _locations = locations;
        }

        public EmployerAccount EmployerAccount { get; private set; }

        public int Amount { get; private set; }

        public bool IsNamePublic { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public JobRole JobRoles { get; private set; }
        
        public Level Levels { get; private set; }

        public Sector Sectors { get; private set; }

        private readonly List<PledgeLocation> _locations;
        public IReadOnlyCollection<PledgeLocation> Locations => _locations;

        public byte[] RowVersion { get; private set; }

        public Application CreateApplication(EmployerAccount account, string details,
            string standardId, int numberOfApprentices, DateTime startDate, bool hasTrainingProvider,
            Sector sectors, string postcode, string firstName, string lastName, string businessWebsite,
            IEnumerable<string> emailAddresses)
        {
            return new Application(this, account, details, standardId, numberOfApprentices, 
                startDate, hasTrainingProvider, sectors, postcode, firstName, lastName,
                businessWebsite, emailAddresses);
        }
    }
}