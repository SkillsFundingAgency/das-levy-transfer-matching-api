using System;

namespace SFA.DAS.LevyTransferMatching.Attributes
{
    public class ReferenceMetadataAttribute : Attribute
    {
        public string Description { get; set; }
        public string Hint { get; set; }
        public string ExtendedDescription { get; set; }
        public string ShortDescription { get; set; }
    }
}
