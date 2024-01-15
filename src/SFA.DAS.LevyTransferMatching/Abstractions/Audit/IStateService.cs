namespace SFA.DAS.LevyTransferMatching.Abstractions.Audit;

public interface IStateService
{
    Dictionary<string, object> GetState(object item);
}