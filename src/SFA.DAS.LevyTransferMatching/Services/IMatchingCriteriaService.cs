using System.Linq;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Services
{
    public interface IMatchingCriteriaService
    {
        MatchingCriteria GetMatchingCriteria(CreateApplicationCommand createApplicationCommand,
            Data.Models.Pledge pledge);
    }

    public class MatchingCriteriaService : IMatchingCriteriaService
    {
        public MatchingCriteria GetMatchingCriteria(CreateApplicationCommand createApplicationCommand, Data.Models.Pledge pledge)
        {
            var sector = false;
            var level = false;
            var location = false;
            var jobRole = false;

            if (!pledge.Locations.Any() || createApplicationCommand.Locations.Any())
            {
                location = true;
            }

            if (!pledge.Sectors.ToList().Any() || createApplicationCommand.Sectors.Any(x => pledge.Sectors.ToList().Contains(x)))
            {
                sector = true;
            }

            if (!pledge.JobRoles.ToList().Any() || pledge.JobRoles.ToList().Any(r => createApplicationCommand.StandardRoute == r.GetDescription()))
            {
                jobRole = true;
            }

            if (!pledge.Levels.ToList().Any() || pledge.Levels.ToList().Select(x => char.GetNumericValue(x.GetShortDescription().Last())).Contains(createApplicationCommand.StandardLevel))
            {
                level = true;
            }

            return new MatchingCriteria(sector, level, location, jobRole);

            //return new MatchingCriteria(false, false, false, false);
        }
    }
}
