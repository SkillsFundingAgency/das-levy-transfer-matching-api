namespace SFA.DAS.LevyTransferMatching.Data.ValueObjects;

public class MatchingCriteria(bool sector, bool level, bool location, bool jobRole)
{
    public bool MatchSector { get; } = sector;
    public bool MatchLevel { get; } = level;
    public bool MatchLocation { get; } = location;
    public bool MatchJobRole { get; } = jobRole;

    public byte MatchPercentage
    {
        get
        {
            var matches = 0;
            if (MatchJobRole) matches++;
            if (MatchLevel) matches++;
            if (MatchLocation) matches++;
            if (MatchSector) matches++;

            return (byte)(matches * 25);
        }
    }
}