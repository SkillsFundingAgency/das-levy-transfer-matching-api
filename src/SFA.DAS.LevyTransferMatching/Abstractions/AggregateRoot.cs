using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Services.Audit;

namespace SFA.DAS.LevyTransferMatching.Abstractions;

public class AggregateRoot<T> : Entity<T>
{
    protected IChangeTrackingSession ChangeTrackingSession { get; private set; }
    private readonly List<Func<IDomainEvent>> _events = new List<Func<IDomainEvent>>();

    protected void StartTrackingSession(UserAction userAction, UserInfo userInfo)
    {
        ChangeTrackingSession = new ChangeTrackingSession(new StateService(), userAction, userInfo);
    }

    protected void AddEvent(Func<IDomainEvent> @event)
    {
        lock (_events)
        {
            _events.Add(@event);
        }
    }

    protected void AddEvent(IDomainEvent @event)
    {
        lock (_events)
        {
            _events.Add(() => @event);
        }
    }

    public IEnumerable<IDomainEvent> FlushEvents()
    {
        lock (_events)
        {
            var result = new List<IDomainEvent>();
            foreach (var eventFunc in _events)
            {
                result.Add(eventFunc.Invoke());
            }
            _events.Clear();

            if(ChangeTrackingSession != null)
            {
                result.AddRange(ChangeTrackingSession.FlushEvents());
            }

            return result;
        }
    }
}