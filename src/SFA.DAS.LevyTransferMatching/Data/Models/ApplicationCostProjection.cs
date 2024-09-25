using SFA.DAS.LevyTransferMatching.Abstractions;

namespace SFA.DAS.LevyTransferMatching.Data.Models;

public class ApplicationCostProjection(string financialYear, int amount) : Entity<int>
{
    public string FinancialYear { get; set; } = financialYear;
    public int Amount { get; set; } = amount;
}