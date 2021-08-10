using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class Application : AggregateRoot<int>
    {
        public Application() {}

        public Application(Pledge pledge, EmployerAccount account, CreateApplicationProperties properties, UserInfo userInfo)
        {
            Pledge = pledge;
            EmployerAccount = account;
            Details = properties.Details;
            StandardId = properties.StandardId;
            NumberOfApprentices = properties.NumberOfApprentices;
            StartDate = properties.StartDate;
            HasTrainingProvider = properties.HasTrainingProvider;
            Amount = properties.Amount;
            Sectors = properties.Sectors;
            Postcode = properties.PostCode;
            FirstName = properties.FirstName;
            LastName = properties.LastName;
            BusinessWebsite = properties.BusinessWebsite;
            CreatedOn = DateTime.UtcNow;
            _emailAddresses = properties.EmailAddresses.Select(x => new ApplicationEmailAddress(x)).ToList();

            StartTrackingSession(UserAction.CreateApplication, account.Id, userInfo);
            ChangeTrackingSession.TrackInsert(this);
            foreach(var emailAddress in _emailAddresses)
            {
                ChangeTrackingSession.TrackInsert(emailAddress);
            }
        }

        public EmployerAccount EmployerAccount { get; private set; }

        public Pledge Pledge { get; private set; }

        public string Details { get; private set; }

        public string StandardId { get; private set; }
        public int NumberOfApprentices { get; private set; }
        public DateTime StartDate { get; private set; }
        public int Amount { get; private set; }
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
