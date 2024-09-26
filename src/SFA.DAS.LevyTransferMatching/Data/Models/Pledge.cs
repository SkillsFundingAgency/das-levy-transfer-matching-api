using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.Data.Models;

public class Pledge : AggregateRoot<int>
{
    protected Pledge()
    {
        _applications = [];
    }

    public Pledge(EmployerAccount employerAccount, CreatePledgeProperties properties, UserInfo userInfo) : this()
    {
        EmployerAccount = employerAccount;
        EmployerAccountId = employerAccount.Id;
        Amount = properties.Amount;
        RemainingAmount = properties.Amount;
        IsNamePublic = properties.IsNamePublic;
        AutomaticApprovalOption = properties.AutomaticApprovalOption;
        Levels = properties.Levels;
        JobRoles = properties.JobRoles;
        Sectors = properties.Sectors;
        CreatedOn = DateTime.UtcNow;
        Status = PledgeStatus.Active;
        _locations = properties.Locations;

        StartTrackingSession(UserAction.CreatePledge, userInfo);
        ChangeTrackingSession.TrackInsert(this);
        foreach (var location in _locations)
        {
            ChangeTrackingSession.TrackInsert(location);
        }
    }

    public long EmployerAccountId { get; private set; }

    public EmployerAccount EmployerAccount { get; private set; }

    public int Amount { get; private set; }

    public int RemainingAmount { get; private set; }

    public bool IsNamePublic { get; private set; }

    public AutomaticApprovalOption AutomaticApprovalOption { get; set; }

    public DateTime CreatedOn { get; private set; }

    public JobRole JobRoles { get; private set; }
        
    public Level Levels { get; private set; }

    public Sector Sectors { get; private set; }
    public PledgeStatus Status { get; private set; }
    public DateTime? ClosedOn { get; private set; }

    private readonly List<PledgeLocation> _locations;
    public IReadOnlyCollection<PledgeLocation> Locations => _locations;

    private readonly List<Application> _applications;
    public IReadOnlyCollection<Application> Applications => _applications;

    public byte[] RowVersion { get; private set; }

    public Application CreateApplication(EmployerAccount account, CreateApplicationProperties properties, UserInfo userInfo)
    {
        ValidateLocationIds(properties.Locations);
        return new Application(this, account, properties, userInfo);
    }

    private bool CanDebit(int debitAmount)
    {
        return RemainingAmount >= debitAmount;
    }

    public bool Debit(int debitAmount, int applicationId, UserInfo userInfo)
    {
        if (!CanDebit(debitAmount))
        {
            AddEvent(new PledgeDebitFailed(Id, applicationId, debitAmount));
            return false;
        }

        if (ShouldPledgeBeAutoClosed(debitAmount))
        {
            ClosePledge(userInfo, true);
        }

        StartTrackingSession(UserAction.DebitPledge, userInfo);
        ChangeTrackingSession.TrackUpdate(this);
        RemainingAmount -= debitAmount;
        return true;
    }

    public void Credit(int creditAmount, UserInfo userInfo)
    {
        StartTrackingSession(UserAction.CreditPledge, userInfo);
        ChangeTrackingSession.TrackUpdate(this);
        RemainingAmount += creditAmount;

        AddEvent(new PledgeCredited(Id));
    }

    public void ClosePledge(UserInfo userInfo, bool insufficientFunds = false)
    {
        if (Status != PledgeStatus.Active)
        {
            throw new InvalidOperationException($"Unable to close Pledge {Id} status {Status}");
        }
            
        StartTrackingSession(UserAction.ClosePledge, userInfo);
        ChangeTrackingSession.TrackUpdate(this);
        Status = PledgeStatus.Closed;
        ClosedOn = DateTime.UtcNow;

        AddEvent(new PledgeClosed(Id, insufficientFunds));
    }

    private void ValidateLocationIds(IEnumerable<int> locationIds)
    {
        if (locationIds == null) return;

        foreach(var locationId in locationIds)
        {
            if(Locations.All(x => x.Id != locationId))
            {
                throw new InvalidOperationException($"Location {locationId} is not valid for pledge {Id}");
            }
        }
    }

    private bool ShouldPledgeBeAutoClosed(int debitAmount)
    {
        var updatedPledgeAmount = RemainingAmount - debitAmount;
        return updatedPledgeAmount > 0 && updatedPledgeAmount < 2000 && Status == PledgeStatus.Active;
    }
}