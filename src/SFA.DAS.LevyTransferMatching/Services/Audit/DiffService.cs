﻿using SFA.DAS.LevyTransferMatching.Abstractions.Audit;

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

    private static List<DiffItem> GenerateDiffForInsert(Dictionary<string, object> updated)
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

    private static List<DiffItem> GenerateDiffForUpdateOrDelete(Dictionary<string, object> initial, Dictionary<string, object> updated)
    {
        var result = new List<DiffItem>();

        foreach (var (key, initialValue) in initial)
        {
            var updatedValue = updated?.GetValueOrDefault(key);

            if (initialValue == null)
            {
                if (updatedValue != null)
                {
                    result.Add(new DiffItem
                    {
                        PropertyName = key,
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
                    PropertyName = key,
                    InitialValue = initialValue,
                    UpdatedValue = null
                });
                continue;
            }

            if (!initialValue.Equals(updatedValue))
            {
                result.Add(new DiffItem
                {
                    PropertyName = key,
                    InitialValue = initialValue,
                    UpdatedValue = updatedValue
                });
            }
        }

        return result;
    }
}