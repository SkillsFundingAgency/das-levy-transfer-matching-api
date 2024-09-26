using Newtonsoft.Json;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Services.Audit;

public class ChangeTrackingSession(IStateService stateService, UserAction userAction, UserInfo userInfo)
    : IChangeTrackingSession
{
    private readonly List<TrackedItem> _trackedItems = [];
    private readonly Guid _correlationId = Guid.NewGuid();

    public void TrackInsert(ITrackableEntity trackedObject)
    {
        _trackedItems.Add(TrackedItem.CreateInsertTrackedItem(trackedObject));
    }

    public void TrackUpdate(ITrackableEntity trackedObject)
    {
        var initialState = stateService.GetState(trackedObject);
        _trackedItems.Add(TrackedItem.CreateUpdateTrackedItem(trackedObject, initialState));
    }
    
    public IEnumerable<IDomainEvent> FlushEvents()
    {
        var result = new List<IDomainEvent>();

        foreach (var item in _trackedItems)
        {
            var updated = item.Operation == ChangeTrackingOperation.Delete ? null : stateService.GetState(item.TrackedEntity);

            result.Add(new EntityStateChanged
            {
                CorrelationId = _correlationId,
                UserAction = userAction,
                EntityType = item.TrackedEntity.GetType().Name,
                EntityId = item.TrackedEntity.GetTrackedEntityId(),
                InitialState = item.InitialState == null ? null : JsonConvert.SerializeObject(item.InitialState),
                UpdatedState = updated == null ? null : JsonConvert.SerializeObject(updated),
                UpdatedOn = DateTime.UtcNow,
                UserId = userInfo?.UserId ?? "Unknown",
                UserDisplayName = userInfo?.UserDisplayName ?? "Unknown"
            });
        }

        _trackedItems.Clear();

        return result;
    }
}

public enum ChangeTrackingOperation
{
    Insert, Delete, Update
}