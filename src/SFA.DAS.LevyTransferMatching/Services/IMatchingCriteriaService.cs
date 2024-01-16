using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Services;

public interface IMatchingCriteriaService
{
    MatchingCriteria GetMatchingCriteria(CreateApplicationCommand application, Pledge pledge);
    MatchingCriteria GetMatchingCriteria(Data.Models.Application application, Pledge pledge);
}

public class MatchingCriteriaService : IMatchingCriteriaService
{
    public MatchingCriteria GetMatchingCriteria(CreateApplicationCommand application, Pledge pledge)
    {
        var location = pledge.Locations.Count == 0 || (application.Locations != null && application.Locations.Count != 0);

        var sector = !pledge.Sectors.ToList().Any() ||
                     application.Sectors.Any(x => pledge.Sectors.ToList().Contains(x));

        var jobRole = !pledge.JobRoles.ToList().Any() || pledge.JobRoles.ToList()
            .Any(r => application.StandardRoute == r.GetDescription());

        var level = !pledge.Levels.ToList().Any() || pledge.Levels.ToList()
            .Select(x => char.GetNumericValue(x.GetShortDescription().Last()))
            .Contains(application.StandardLevel);

        return new MatchingCriteria(sector, level, location, jobRole);
    }

    public MatchingCriteria GetMatchingCriteria(Data.Models.Application application, Pledge pledge)
    {
        var location = pledge.Locations.Count == 0 || application.ApplicationLocations.Count != 0;

        var sector = !pledge.Sectors.ToList().Any() ||
                     application.Sectors.ToList().Any(x => pledge.Sectors.ToList().Contains(x));

        var jobRole = !pledge.JobRoles.ToList().Any() || pledge.JobRoles.ToList()
            .Any(r => application.StandardRoute == r.GetDescription());

        var level = !pledge.Levels.ToList().Any() || pledge.Levels.ToList()
            .Select(x => char.GetNumericValue(x.GetShortDescription().Last()))
            .Contains(application.StandardLevel);

        return new MatchingCriteria(sector, level, location, jobRole);
    }
}