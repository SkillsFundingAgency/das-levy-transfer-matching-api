namespace SFA.DAS.LevyTransferMatching.Abstractions.CustomExceptions;

[Serializable]
public class AggregateNotFoundException<T> : Exception
{
    public AggregateNotFoundException(long id) : base($"{nameof(T)} {id} not found") { }
}