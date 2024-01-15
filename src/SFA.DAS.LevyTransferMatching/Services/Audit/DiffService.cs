using SFA.DAS.LevyTransferMatching.Abstractions.Audit;

namespace SFA.DAS.LevyTransferMatching.Services.Audit;

public class DiffService : IDiffService
{
    public IReadOnlyList<DiffItem> GenerateDiff(Dictionary<string, object> initial, Dictionary<string, object> updated)
    {
        if (initial == null && updated == null)
        {
            return new List<DiffItem>();
        }

        if (initial == null)
        {
            return GenerateDiffForInsert(updated);
        }

        return GenerateDiffForUpdateOrDelete(initial, updated);
    }

    private static IReadOnlyList<DiffItem> GenerateDiffForInsert(Dictionary<string, object> updated)
    {
        var result = new List<DiffItem>();

        foreach (var item in updated.Where(x => x.Value != null))
        {
            result.Add(new DiffItem
            {
                PropertyName = item.Key,
                InitialValue = null,
                UpdatedValue = item.Value
            });
        }

        return result;
    }

    private static IReadOnlyList<DiffItem> GenerateDiffForUpdateOrDelete(Dictionary<string, object> initial,
        Dictionary<string, object> updated)
    {
        var result = new List<DiffItem>();

        foreach (var item in initial)
        {
            var initialValue = item.Value;
            var updatedValue = updated == null ? null : updated.ContainsKey(item.Key) ? updated[item.Key] : null;

            if (initialValue == null)
            {
                if (updatedValue != null)
                {
                    result.Add(new DiffItem
                    {
                        PropertyName = item.Key,
                        InitialValue = null,
                        UpdatedValue = updatedValue
                    });
                }
                continue;
            }

            if (updatedValue == null)
            {
                result.Add(new DiffItem
                {
                    PropertyName = item.Key,
                    InitialValue = initialValue,
                    UpdatedValue = null
                });
                continue;
            }

            if (!initialValue.Equals(updatedValue))
            {
                result.Add(new DiffItem
                {
                    PropertyName = item.Key,
                    InitialValue = initialValue,
                    UpdatedValue = updatedValue
                });
            }
        }

        return result;
    }
}