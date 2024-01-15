using System;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;

namespace SFA.DAS.LevyTransferMatching.Abstractions;

public class Entity<T> : ITrackableEntity
{
    public T Id { get; protected set; }

    public long GetTrackedEntityId()
    {
        return Convert.ToInt64(Id);
    }
}