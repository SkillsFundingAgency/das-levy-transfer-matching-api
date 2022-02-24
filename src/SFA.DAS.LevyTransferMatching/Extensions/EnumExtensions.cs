using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Attributes;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var displayAttribute = GetMetadataAttribute(value);
            return displayAttribute == null ? value.ToString() : displayAttribute.Description;
        }

        public static string GetShortDescription(this Enum value)
        {
            var displayAttribute = GetMetadataAttribute(value);
            return displayAttribute == null ? "" : displayAttribute.ShortDescription;
        }

        public static string GetHint(this Enum value)
        {
            var displayAttribute = GetMetadataAttribute(value);
            return displayAttribute == null ? "" : displayAttribute.Hint;
        }

        private static ReferenceMetadataAttribute GetMetadataAttribute(Enum value)
        {
            var type = value.GetType();

            var members = type.GetMember(value.ToString());
            if (members.Length == 0) throw new ArgumentException($"error '{value}' not found in type '{type.Name}'");

            var member = members[0];
            var attributes = member.GetCustomAttributes(typeof(ReferenceMetadataAttribute), false);
            if (attributes.Length > 0)
            {
                return (ReferenceMetadataAttribute)attributes[0];
            }

            return null;
        }

        public static IEnumerable<ReferenceDataItem> ConvertToReferenceData<T>() where T : Enum
        {
            var result = new List<ReferenceDataItem>();

            var enumType = typeof(T);

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                result.Add(new ReferenceDataItem
                {
                    Id = enumValue.ToString(),
                    Description = enumValue.GetDescription(),
                    Hint = enumValue.GetHint(),
                    ShortDescription = enumValue.GetShortDescription()
                });
            }

            return result;
        }

        public static IEnumerable<TEnum> ToList<TEnum>(this TEnum value) where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Where(x => value.HasFlag(x));
        }
    }
}
