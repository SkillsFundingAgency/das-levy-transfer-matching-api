using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class Application : AggregateRoot<int>
    {
        protected Application()
        {
            _statusHistory = new List<ApplicationStatusHistory>();
            _applicationLocations = new List<ApplicationLocation>();
        }

        public Application(Pledge pledge, EmployerAccount account, CreateApplicationProperties properties, UserInfo userInfo) : this()
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
            FirstName = properties.FirstName;
            LastName = properties.LastName;
            BusinessWebsite = properties.BusinessWebsite;
            CreatedOn = DateTime.UtcNow;

            _emailAddresses = properties.EmailAddresses.Select(x => new ApplicationEmailAddress(x)).ToList();

            if (properties.Locations != null)
            {
                _applicationLocations = properties.Locations.Select(x => new ApplicationLocation(x)).ToList();
            }

            AdditionalLocation = properties.AdditionalLocation;
            SpecificLocation = properties.SpecificLocation;

            StartTrackingSession(UserAction.CreateApplication, userInfo);
            ChangeTrackingSession.TrackInsert(this);
            foreach(var emailAddress in _emailAddresses)
            {
                ChangeTrackingSession.TrackInsert(emailAddress);
            }
            foreach (var location in _applicationLocations)
            {
                ChangeTrackingSession.TrackInsert(location);
            }

            AddStatusHistory(CreatedOn);
        }

        public EmployerAccount EmployerAccount { get; private set; }

        public int PledgeId { get; private set; }
        public Pledge Pledge { get; private set; }

        public string Details { get; private set; }

        public string StandardId { get; private set; }
        public int NumberOfApprentices { get; private set; }
        public DateTime StartDate { get; private set; }
        public int Amount { get; private set; }
        public bool HasTrainingProvider { get; private set; }

        public Sector Sectors { get; private set; }
        public string AdditionalLocation { get; set; }
        public string SpecificLocation { get; set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string BusinessWebsite { get; private set; }

        public int NumberOfApprenticesUsed { get; private set; }
        public int AmountUsed { get; private set; }

        private readonly List<ApplicationEmailAddress> _emailAddresses;
        public IReadOnlyCollection<ApplicationEmailAddress> EmailAddresses => _emailAddresses;

        private readonly List<ApplicationStatusHistory> _statusHistory;
        public IReadOnlyCollection<ApplicationStatusHistory> StatusHistory => _statusHistory;

        private readonly List<ApplicationLocation> _applicationLocations;
        public IReadOnlyCollection<ApplicationLocation> ApplicationLocations => _applicationLocations;

        public DateTime CreatedOn { get; private set; }
        public ApplicationStatus Status { get; private set; }
        public DateTime? UpdatedOn { get; private set; }

        public byte[] RowVersion { get; private set; }

        public void Approve(UserInfo userInfo)
        {
            if (Status != ApplicationStatus.Pending)
            {
                throw new InvalidOperationException($"Unable to approve Application {Id} status {Status}");
            }

            StartTrackingSession(UserAction.ApproveApplication, userInfo);
            ChangeTrackingSession.TrackUpdate(this);
            Status = ApplicationStatus.Approved;
            UpdatedOn = DateTime.UtcNow;
            AddEvent(new ApplicationApproved(Id, PledgeId, UpdatedOn.Value, Amount));

            AddStatusHistory(UpdatedOn.Value);
        }

        public void AcceptFunding(UserInfo userInfo)
        {
            if (Status != ApplicationStatus.Approved)
            {
                throw new InvalidOperationException($"Unable to accept funding for Application {Id} status {Status}");
            }

            StartTrackingSession(UserAction.AcceptFunding, userInfo);
            ChangeTrackingSession.TrackUpdate(this);
            Status = ApplicationStatus.Accepted;
            UpdatedOn = DateTime.UtcNow;

            AddStatusHistory(UpdatedOn.Value);
        }

        public void DeclineFunding(UserInfo userInfo)
        {
            if (Status != ApplicationStatus.Approved)
            {
                throw new InvalidOperationException($"Unable to accept funding for Application {Id} status {Status}");
            }

            StartTrackingSession(UserAction.DeclineFunding, userInfo);
            ChangeTrackingSession.TrackUpdate(this);
            Status = ApplicationStatus.Declined;
            UpdatedOn = DateTime.UtcNow;
            AddEvent(new ApplicationFundingDeclined(Id, PledgeId, UpdatedOn.Value, Amount));

            AddStatusHistory(UpdatedOn.Value);
        }

        public void UndoApproval(UserInfo userInfo)
        {
            if (Status != ApplicationStatus.Approved)
            {
                throw new InvalidOperationException($"Unable to undo approval of Application {Id} status {Status}");
            }

            StartTrackingSession(UserAction.UndoApplicationApproval, userInfo);
            ChangeTrackingSession.TrackUpdate(this);
            Status = ApplicationStatus.Pending;
            UpdatedOn = DateTime.UtcNow;

            AddStatusHistory(UpdatedOn.Value);
        }

        private void AddStatusHistory(DateTime date)
        {
            _statusHistory.Add(new ApplicationStatusHistory(Status, date));
        }

        public void Debit(int numberOfApprenticesUsed, int amountUsed, int maxAmount, UserInfo userInfo)
        {
            if (Status != ApplicationStatus.Accepted)
                throw new InvalidOperationException($"Unable to debit application with Id: {Id}. Application status is {Status} when it should be {ApplicationStatus.Accepted}.");

            StartTrackingSession(UserAction.DebitApplication, userInfo);
            ChangeTrackingSession.TrackUpdate(this);

            NumberOfApprenticesUsed += numberOfApprenticesUsed;
            AmountUsed += amountUsed;

            if (NumberOfApprenticesUsed >= NumberOfApprentices || AmountUsed >= maxAmount)
                Status = ApplicationStatus.FundsUsed;

            UpdatedOn = DateTime.UtcNow;
        }
    }
}
