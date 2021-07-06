using SFA.DAS.LevyTransferMatching.Attributes;

namespace SFA.DAS.LevyTransferMatching.Models.Enums
{
    public enum Level
    {
        [ReferenceMetadata(Description = "Level 2 - GCSE", ShortDescription = "2")]
        Level2 = 1,
        [ReferenceMetadata(Description = "Level 3 - A level", ShortDescription = "3")]
        Level3 = 2,
        [ReferenceMetadata(Description = "Level 4 - higher national certificate (HNC)", ShortDescription = "4")]
        Level4 = 4,
        [ReferenceMetadata(Description = "Level 5 - higher national diploma (HND)", ShortDescription = "5")]
        Level5 = 8,
        [ReferenceMetadata(Description = "Level 6 - degree", ShortDescription = "6")]
        Level6 = 16,
        [ReferenceMetadata(Description = "Level 7 - master’s degree", ShortDescription = "7")]
        Level7 = 32,
    }
}