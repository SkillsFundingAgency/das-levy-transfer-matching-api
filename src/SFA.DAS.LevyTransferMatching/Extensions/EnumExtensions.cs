using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.LevyTransferMatching.Models.Tags;

namespace SFA.DAS.LevyTransferMatching.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var displayAttribute = GetDisplayAttribute(value);
            return displayAttribute == null ? value.ToString() : displayAttribute.GetName();
        }

        public static string GetDescription(this Enum value)
        {
            var displayAttribute = GetDisplayAttribute(value);
            return displayAttribute == null ? "" : displayAttribute.GetDescription();
        }

        private static DisplayAttribute GetDisplayAttribute(Enum value)
        {
            var type = value.GetType();

            var members = type.GetMember(value.ToString());
            if (members.Length == 0) throw new ArgumentException($"error '{value}' not found in type '{type.Name}'");

            var member = members[0];
            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length > 0)
            {
                return (DisplayAttribute)attributes[0];
            }

            return null;
        }

        public static IEnumerable<Tag> ConvertToTags<T>() where T : Enum
        {
            var result = new List<Tag>();

            var enumType = typeof(T);

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                result.Add(new Tag
                {
                    TagId = enumValue.ToString(),
                    Description = enumValue.GetDisplayName(),
                    ExtendedDescription = enumValue.GetDescription()
                });
            }

            return result;
        }
    }
}
