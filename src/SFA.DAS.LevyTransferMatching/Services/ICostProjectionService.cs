using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Services
{
    public interface ICostProjectionService
    {
        List<CostProjection> GetCostProjections(int totalAmount, DateTime startDate, int duration);
    }

    public class CostProjectionService : ICostProjectionService
    {
        public List<CostProjection> GetCostProjections(int totalAmount, DateTime startDate, int duration)
        {
            var result = new Dictionary<string, decimal>();

            var total = (decimal) totalAmount;
            var completionPayment = total / 5;

            var monthly = (total - completionPayment) / duration;

            var workingDate = startDate;

            for (var i = 0; i < duration; i++)
            {
                var financialYear = workingDate.GetLastDayOfMonth().GetFinancialYear();

                decimal currentAmount = 0;

                if (result.ContainsKey(financialYear))
                {
                    currentAmount = result[financialYear];
                }
                else
                {
                    result.Add(financialYear, 0);
                }

                currentAmount += monthly;

                if (i == duration-1)
                {
                    currentAmount += completionPayment;
                }

                result[financialYear] = currentAmount;

                workingDate = workingDate.AddMonths(1);
            }

            return result.Select(x => new CostProjection(x.Key, x.Value.ToNearest(100))).ToList();
        }
    }
}
