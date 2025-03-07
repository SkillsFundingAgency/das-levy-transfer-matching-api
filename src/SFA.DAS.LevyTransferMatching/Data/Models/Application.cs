﻿using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.Models;

public class Application : AggregateRoot<int>
{
    protected Application()
    {
        _statusHistory = [];
        _applicationLocations = [];
        _applicationCostProjections = [];
    }

    public Application(Pledge pledge, EmployerAccount account, CreateApplicationProperties properties, UserInfo userInfo) : this()
    {
        Pledge = pledge;
        EmployerAccount = account;
        Details = properties.Details;
        StandardId = properties.StandardId;
        StandardTitle = properties.StandardTitle;
        StandardLevel = properties.StandardLevel;
        StandardDuration = properties.StandardDuration;
        StandardMaxFunding = properties.StandardMaxFunding;
        StandardRoute = properties.StandardRoute;
        MatchJobRole = properties.MatchingCriteria.MatchJobRole;
        MatchLevel = properties.MatchingCriteria.MatchLevel;
        MatchLocation = properties.MatchingCriteria.MatchLocation;
        MatchSector = properties.MatchingCriteria.MatchSector;
        MatchPercentage = properties.MatchingCriteria.MatchPercentage;
        NumberOfApprentices = properties.NumberOfApprentices;
        StartDate = properties.StartDate;
        HasTrainingProvider = properties.HasTrainingProvider;
        Sectors = properties.Sectors;
        FirstName = properties.FirstName;
        LastName = properties.LastName;
        BusinessWebsite = properties.BusinessWebsite;
        TotalAmount = properties.StandardMaxFunding * properties.NumberOfApprentices;
        CreatedOn = DateTime.UtcNow;

        _emailAddresses = properties.EmailAddresses.Select(x => new ApplicationEmailAddress(x)).ToList();

        if (properties.Locations != null)
        {
            _applicationLocations = properties.Locations.Select(x => new ApplicationLocation(x)).ToList();
        }

        _applicationCostProjections = properties.CostProjections.Select(x => new ApplicationCostProjection(x.FinancialYear, x.Amount)).ToList();

        AdditionalLocation = properties.AdditionalLocation;
        SpecificLocation = properties.SpecificLocation;

        CostingModel = properties.CostingModel;

        StartTrackingSession(UserAction.CreateApplication, userInfo);
        ChangeTrackingSession.TrackInsert(this);
        foreach (var emailAddress in _emailAddresses)
        {
            ChangeTrackingSession.TrackInsert(emailAddress);
        }
        foreach (var location in _applicationLocations)
        {
            ChangeTrackingSession.TrackInsert(location);
        }

        AddEvent(() => new ApplicationCreated(Id, Pledge.Id, EmployerAccount.Id, CreatedOn));

        AddStatusHistory(CreatedOn);
    }

    public EmployerAccount EmployerAccount { get; private set; }

    public int PledgeId { get; private set; }
    public Pledge Pledge { get; private set; }

    public string Details { get; private set; }

    public string StandardId { get; private set; }
    public string StandardTitle { get; private set; }
    public int StandardLevel { get; private set; }
    public int StandardDuration { get; private set; }
    public int StandardMaxFunding { get; private set; }
    public string StandardRoute { get; private set; }

    public bool MatchSector { get; private set; }

    public bool MatchLevel { get; private set; }

    public bool MatchLocation { get; private set; }

    public bool MatchJobRole { get; private set; }

    public byte MatchPercentage { get; private set; }

    public int NumberOfApprentices { get; private set; }
    public DateTime StartDate { get; private set; }
    [Obsolete("Application costs are now dynamic and subject to change, so values previously persisted in the Amount field should no longer be used")]
    public int Amount { get; private set; }
    public int TotalAmount { get; private set; }
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

    private readonly List<ApplicationCostProjection> _applicationCostProjections;
    public IReadOnlyCollection<ApplicationCostProjection> ApplicationCostProjections => _applicationCostProjections;

    public DateTime CreatedOn { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public bool AutomaticApproval { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public ApplicationCostingModel CostingModel { get; private set; }
    public byte[] RowVersion { get; private set; }

    public void Approve(UserInfo userInfo, bool automaticApproval = false)
    {
        if (Status != ApplicationStatus.Pending)
        {
            throw new InvalidOperationException($"Unable to approve Application {Id} status {Status}");
        }

        StartTrackingSession(UserAction.ApproveApplication, userInfo);
        ChangeTrackingSession.TrackUpdate(this);
        Status = ApplicationStatus.Approved;
        AutomaticApproval = automaticApproval;
        UpdatedOn = DateTime.UtcNow;

        AddEvent(new ApplicationApproved(Id, PledgeId, UpdatedOn.Value, GetCost(), EmployerAccount.Id));

        AddStatusHistory(UpdatedOn.Value);
    }

    public void Reject(UserInfo userInfo)
    {
        if (Status != ApplicationStatus.Pending)
        {
            throw new InvalidOperationException($"Unable to reject Application {Id} status {Status}");
        }

        StartTrackingSession(UserAction.RejectApplication, userInfo);
        ChangeTrackingSession.TrackUpdate(this);
        Status = ApplicationStatus.Rejected;
        UpdatedOn = DateTime.UtcNow;

        AddEvent(new ApplicationRejected(Id, PledgeId, UpdatedOn.Value, GetCost(), EmployerAccount.Id));

        AddStatusHistory(UpdatedOn.Value);
    }

    public void AcceptFunding(UserInfo userInfo, bool shouldRejectApplications = false)
    {
        if (Status != ApplicationStatus.Approved)
        {
            throw new InvalidOperationException($"Unable to accept funding for Application {Id} status {Status}");
        }

        StartTrackingSession(UserAction.AcceptFunding, userInfo);
        ChangeTrackingSession.TrackUpdate(this);
        Status = ApplicationStatus.Accepted;
        UpdatedOn = DateTime.UtcNow;

        AddEvent(new ApplicationFundingAccepted(Id, PledgeId, shouldRejectApplications));

        AddStatusHistory(UpdatedOn.Value);
    }

    public void DeclineFunding(UserInfo userInfo)
    {
        if (Status != ApplicationStatus.Approved)
        {
            throw new InvalidOperationException($"Unable to decline funding for Application {Id} status {Status}");
        }

        StartTrackingSession(UserAction.DeclineFunding, userInfo);
        ChangeTrackingSession.TrackUpdate(this);
        Status = ApplicationStatus.Declined;
        UpdatedOn = DateTime.UtcNow;

        AddEvent(new ApplicationFundingDeclined(Id, PledgeId, UpdatedOn.Value, GetCost()));

        AddStatusHistory(UpdatedOn.Value);
    }

    public void ExpireAcceptedFunding(UserInfo userInfo)
    {
        if (Status != ApplicationStatus.Accepted)
        {
            throw new InvalidOperationException($"Unable to expire funding for Application {Id} status {Status}");
        }

        StartTrackingSession(UserAction.ExpireAcceptedFunding, userInfo);

        ChangeTrackingSession.TrackUpdate(this);
        Status = ApplicationStatus.FundsExpired;

        UpdatedOn = DateTime.UtcNow;
        AddEvent(new ApplicationFundingExpired(Id, PledgeId, GetCost()));
        AddStatusHistory(UpdatedOn.Value);
    }

    public void Withdraw(UserInfo userInfo)
    {
        if (Status == ApplicationStatus.Pending)
        {
            StartTrackingSession(UserAction.WithdrawApplication, userInfo);
            ChangeTrackingSession.TrackUpdate(this);
            Status = ApplicationStatus.Withdrawn;
            UpdatedOn = DateTime.UtcNow;
            AddEvent(new ApplicationWithdrawn(Id, PledgeId, UpdatedOn.Value));
        }
        else if (Status == ApplicationStatus.Accepted)
        {
            StartTrackingSession(UserAction.WithdrawApplicationAfterAcceptance, userInfo);
            ChangeTrackingSession.TrackUpdate(this);
            Status = ApplicationStatus.WithdrawnAfterAcceptance;
            UpdatedOn = DateTime.UtcNow;

            AddEvent(new ApplicationWithdrawnAfterAcceptance(Id, PledgeId, GetCost()));
        }
        else
        {
            throw new InvalidOperationException($"Unable to withdraw application with Id: {Id}. Application status is {Status} when it should be {ApplicationStatus.Pending} or {ApplicationStatus.Accepted}");
        }

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
        if (Status != ApplicationStatus.Accepted && Status != ApplicationStatus.FundsUsed)
            throw new InvalidOperationException($"Unable to debit application Id {Id}. Application status is {Status} and must be one of {ApplicationStatus.Accepted}, {ApplicationStatus.FundsUsed}");

        StartTrackingSession(UserAction.DebitApplication, userInfo);
        ChangeTrackingSession.TrackUpdate(this);

        NumberOfApprenticesUsed += numberOfApprenticesUsed;
        AmountUsed += amountUsed;

        if (NumberOfApprenticesUsed >= NumberOfApprentices || AmountUsed >= maxAmount)
            Status = ApplicationStatus.FundsUsed;

        UpdatedOn = DateTime.UtcNow;
    }

    public void SetCostProjection(IEnumerable<CostProjection> costProjections)
    {
        _applicationCostProjections.Clear();
        _applicationCostProjections.AddRange(costProjections.Select(x => new ApplicationCostProjection(x.FinancialYear, x.Amount)).ToList());
    }

    public int GetCost()
    {
        if (CostingModel == ApplicationCostingModel.Original)
        {
            var statusChangeDate = StatusHistory.FirstOrDefault(x =>
                x.Status == ApplicationStatus.Approved || x.Status == ApplicationStatus.Rejected);

            var costDate = statusChangeDate?.CreatedOn ?? DateTime.Now;

            return ApplicationCostProjections.FirstOrDefault(p => p.FinancialYear == costDate.GetFinancialYear())?.Amount ?? 0;
        }

        if (NumberOfApprentices == 0) return 0;

        if (StandardDuration <= 12)
        {
            return StandardMaxFunding * NumberOfApprentices;
        }

        var fundingBandMax = StandardMaxFunding * NumberOfApprentices * 0.8m;
        return ((fundingBandMax / StandardDuration) * 12).ToNearest(1);
    }
}