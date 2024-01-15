namespace SFA.DAS.LevyTransferMatching.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

public static class IntExtensions
{
    public static IEnumerable<TEnum> GetFlags<TEnum>(this int value) where TEnum : Enum
    {
        var combinedEnum = (TEnum)(object)value;

        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
            .Where(x => combinedEnum.HasFlag(x)); ;
    }
}