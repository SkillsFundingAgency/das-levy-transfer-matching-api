using SFA.DAS.LevyTransferMatching.Abstractions;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class ApplicationCostProjection : Entity<int>
    {
        public ApplicationCostProjection() {}

        public ApplicationCostProjection(string financialYear, int amount)
        {
            FinancialYear = financialYear;
            Amount = amount;
        }

        public string FinancialYear { get; set; }
        public int Amount { get; set; }
    }
}
