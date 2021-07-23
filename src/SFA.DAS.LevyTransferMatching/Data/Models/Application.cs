using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class Application : AggregateRoot
    {
        public Application() {}

        public Application(Pledge pledge, EmployerAccount account, string details, string standardId,
            int numberOfApprentices, DateTime startDate, bool hasTrainingProvider, Sector sectors,
            string postcode, string firstName, string lastName, string businessWebsite,
            IEnumerable<string> emailAddresses)
        {
            Pledge = pledge;
            EmployerAccount = account;
            Details = details;
            StandardId = standardId;
            NumberOfApprentices = numberOfApprentices;
            StartDate = startDate;
            HasTrainingProvider = hasTrainingProvider;
            Sectors = sectors;
            Postcode = postcode;
            FirstName = firstName;
            LastName = lastName;
            BusinessWebsite = businessWebsite;
            CreatedOn = DateTime.UtcNow;
            _emailAddresses = emailAddresses.Select(x => new ApplicationEmailAddress(x)).ToList();
        }

        public int Id { get; private set; }

        public EmployerAccount EmployerAccount { get; private set; }

        public Pledge Pledge { get; private set; }

        public string Details { get; private set; }

        public string StandardId { get; private set; }
        public int NumberOfApprentices { get; private set; }
        public DateTime StartDate { get; private set; }
        public bool HasTrainingProvider { get; private set; }

        public Sector Sectors { get; private set; }
        public string Postcode { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string BusinessWebsite { get; private set; }

        private readonly List<ApplicationEmailAddress> _emailAddresses;
        public IReadOnlyCollection<ApplicationEmailAddress> EmailAddresses => _emailAddresses;

        public DateTime CreatedOn { get; private set; }

        public byte[] RowVersion { get; private set; }
    }
}
