using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Models;

public static class OpportunitiesSortBy
{
    public const string ValueLowToHigh = "ValueLowToHigh";
    public const string ValueHighToLow = "ValueHighToLow";
    public const string MostRecent = "MostRecent";
    public const string AtoZ = "AtoZ";
    public const string ZtoA = "ZtoA";

    public static IOrderedQueryable<Pledge> ApplySorting(IQueryable<Pledge> query, string sortBy)
    {
        return sortBy switch
        {
            ValueLowToHigh => query.OrderBy(x => x.RemainingAmount),
            ValueHighToLow => query.OrderByDescending(x => x.RemainingAmount),
            MostRecent => query.OrderByDescending(x => x.CreatedOn),
            AtoZ => query.OrderBy(x => x.EmployerAccount.Name),
            ZtoA => query.OrderByDescending(x => x.EmployerAccount.Name),
            _ => query.OrderByDescending(x => x.RemainingAmount),
        };
    }
}
