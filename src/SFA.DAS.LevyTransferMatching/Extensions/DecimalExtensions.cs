using System;

namespace SFA.DAS.LevyTransferMatching.Extensions
{
    public static class DecimalExtensions
    {
        public static int ToNearest(this decimal value, int roundTo)
        {
            return (int)Math.Round(value / roundTo, MidpointRounding.AwayFromZero) * roundTo;
        }
    }
}
