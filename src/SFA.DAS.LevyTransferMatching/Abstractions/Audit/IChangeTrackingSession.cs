using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Abstractions.Audit;

public interface IChangeTrackingSession
{
    void TrackInsert(ITrackableEntity trackedObject);
    void TrackUpdate(ITrackableEntity trackedObject);
    IEnumerable<IDomainEvent> FlushEvents();
}