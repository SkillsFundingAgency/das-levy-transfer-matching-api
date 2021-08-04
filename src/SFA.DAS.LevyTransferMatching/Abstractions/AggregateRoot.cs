using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Services.Audit;

namespace SFA.DAS.LevyTransferMatching.Abstractions
{
    public class AggregateRoot<T> : Entity<T>
    {
        protected IChangeTrackingSession ChangeTrackingSession { get; private set; }
        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();

        protected void StartTrackingSession(UserAction userAction, long employerAccountId, string userId, string userDisplayName)
        {
            ChangeTrackingSession = new ChangeTrackingSession(new StateService(), userAction, employerAccountId, userId, userDisplayName);
        }

        protected void AddEvent(IDomainEvent @event)
        {
            lock (_events)
            {
                _events.Add(@event);
            }
        }

        public IEnumerable<IDomainEvent> FlushEvents()
        {
            lock (_events)
            {
                var result = new List<IDomainEvent>(_events);
                _events.Clear();
                result.AddRange(ChangeTrackingSession.FlushEvents());
                return result;
            }
        }
    }
}
