namespace SFA.DAS.LevyTransferMatching.Data.ValueObjects
{
    public class MatchingCriteria
    {
        public MatchingCriteria(bool sector, bool level, bool location, bool jobRole)
        {
            MatchSector = sector;
            MatchLevel = level;
            MatchLocation = location;
            MatchJobRole = jobRole;
        }

        public bool MatchSector { get; }
        public bool MatchLevel { get; }
        public bool MatchLocation { get; }
        public bool MatchJobRole { get; }

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
}
