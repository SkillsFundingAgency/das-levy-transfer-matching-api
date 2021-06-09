using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LevyTransferMatching.Models.Enums
{
    public enum Level
    {
        [Display(Name = "Level 2 - GCSE")]
        Level2 = 1,
        [Display(Name = "Level 3 - A level")]
        Level3 = 2,
        [Display(Name = "Level 4 - higher national cerificate (HNC)")]
        Level4 = 3,
        [Display(Name = "Level 5 - higher national diploma (HND)")]
        Level5 = 4,
        [Display(Name = "Level 6 - degree")]
        Level6 = 5,
        [Display(Name = "Level 7 - master’s degree")]
        Level7 = 6,
    }
}