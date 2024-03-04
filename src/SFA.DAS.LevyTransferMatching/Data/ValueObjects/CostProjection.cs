namespace SFA.DAS.LevyTransferMatching.Data.ValueObjects;

public class CostProjection
{
    public CostProjection(string financialYear, int amount)
    {
        FinancialYear = financialYear;
        Amount = amount;
    }

    public string FinancialYear { get; }
    public int Amount { get; }
}