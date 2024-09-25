namespace SFA.DAS.LevyTransferMatching.Data.ValueObjects;

public class CostProjection(string financialYear, int amount)
{
    public string FinancialYear { get; } = financialYear;
    public int Amount { get; } = amount;
}