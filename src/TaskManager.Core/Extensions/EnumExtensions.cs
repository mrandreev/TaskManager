using System.ComponentModel;
using System.Reflection;
using TaskManager.Core.Models;

namespace TaskManager.Core.Extensions;

public static class EnumExtensions
{
    public static EnumModel ToEnumModel(this Enum value)
    {
        return new EnumModel
        {
            Id = Convert.ToInt32(value),
            Name = value.GetDescriptionOrName()
        };
    }

    private static string GetDescriptionOrName(this Enum value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var descriptionAttribute = memberInfo?.GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttribute?.Description ?? value.ToString();
    }
}