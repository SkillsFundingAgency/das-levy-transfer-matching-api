namespace SFA.DAS.LevyTransferMatching.Abstractions.Audit;

public interface IDiffService
{
    IReadOnlyList<DiffItem> GenerateDiff(Dictionary<string, object> initial, Dictionary<string, object> updated);
}