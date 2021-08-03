using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.LevyTransferMatching.Services.Audit
{
    public class ChangeTrackingSession : IChangeTrackingSession
    {
        private readonly IStateService _stateService;
        private readonly List<TrackedItem> _trackedItems;
        private readonly Guid _correlationId;
        private readonly UserAction _userAction;
        private readonly long _employerAccountId;
        private readonly string _userId;
        private readonly string _userDisplayName;

        public ChangeTrackingSession(IStateService stateService, UserAction userAction, long employerAccountId, string userId, string userDisplayName)
        {
            _stateService = stateService;
            _userAction = userAction;
            _employerAccountId = employerAccountId;
            _userId = userId;
            _userDisplayName = userDisplayName;
            _correlationId = Guid.NewGuid();
            _trackedItems = new List<TrackedItem>();
        }

        public IReadOnlyList<TrackedItem> TrackedItems => _trackedItems.AsReadOnly();

        public void TrackInsert(ITrackableEntity trackedObject)
        {
            _trackedItems.Add(TrackedItem.CreateInsertTrackedItem(trackedObject));
        }

        public void TrackUpdate(ITrackableEntity trackedObject)
        {
            var initialState = _stateService.GetState(trackedObject);
            _trackedItems.Add(TrackedItem.CreateUpdateTrackedItem(trackedObject, initialState));
        }

        public void TrackDelete(ITrackableEntity trackedObject)
        {
            var initialState = _stateService.GetState(trackedObject);
            _trackedItems.Add(TrackedItem.CreateDeleteTrackedItem(trackedObject, initialState));
        }

        public IEnumerable<IDomainEvent> FlushEvents()
        {
            var result = new List<IDomainEvent>();

            foreach (var item in _trackedItems)
            {
                var updated = item.Operation == ChangeTrackingOperation.Delete ? null : _stateService.GetState(item.TrackedEntity);

                result.Add(new EntityStateChanged
                    {
                        CorrelationId = _correlationId,
                        StateChangeType = _userAction,
                        EntityType = item.TrackedEntity.GetType().Name,
                        EntityId = item.TrackedEntity.GetTrackedEntityId(),
                        EmployerAccountId = _employerAccountId,
                        InitialState = item.InitialState == null ? null : JsonConvert.SerializeObject(item.InitialState),
                        UpdatedState = updated == null ? null : JsonConvert.SerializeObject(updated),
                        UpdatedOn = DateTime.UtcNow,
                        UserId = _userId ?? "Unknown",
                        UserName = _userDisplayName ?? "Unknown"
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
}
