﻿using Newtonsoft.Json;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class EntityStateChangedHandler(LevyTransferMatchingDbContext dbContext, IDiffService diffService)
    : IDomainEventHandler<EntityStateChanged>
{
    public async Task Handle(EntityStateChanged @event, CancellationToken cancellationToken = default)
    {
        var initialState = @event.InitialState == null
            ? null
            : JsonConvert.DeserializeObject<Dictionary<string, object>>(@event.InitialState);

        var updatedState = @event.UpdatedState == null
            ? null
            : JsonConvert.DeserializeObject<Dictionary<string, object>>(@event.UpdatedState);

        var diff = diffService.GenerateDiff(initialState, updatedState);

        if (diff.Count == 0) return;

        var audit = new Audit
        {
            EntityType = @event.EntityType,
            EntityId = @event.EntityId,
            UserId = @event.UserId,
            UserDisplayName = @event.UserDisplayName,
            UserAction = @event.UserAction.ToString(),
            AuditDate = DateTime.UtcNow,
            InitialState = @event.InitialState,
            UpdatedState = @event.UpdatedState,
            Diff = JsonConvert.SerializeObject(diff),
            CorrelationId = @event.CorrelationId
        };

        await dbContext.Audits.AddAsync(audit, cancellationToken);
    }
}