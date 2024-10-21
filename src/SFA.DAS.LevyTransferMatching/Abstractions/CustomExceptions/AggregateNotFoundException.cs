namespace SFA.DAS.LevyTransferMatching.Abstractions.CustomExceptions;

[Serializable]
public class AggregateNotFoundException<T>(long id) : Exception($"{nameof(T)} {id} not found");