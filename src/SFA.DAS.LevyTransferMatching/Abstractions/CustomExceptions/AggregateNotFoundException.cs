using System;
using System.Runtime.Serialization;

namespace SFA.DAS.LevyTransferMatching.Abstractions.CustomExceptions;

[Serializable]
public class AggregateNotFoundException<T> : Exception
{
    protected AggregateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    public AggregateNotFoundException(long id) : base($"{nameof(T)} {id} not found") { }
}