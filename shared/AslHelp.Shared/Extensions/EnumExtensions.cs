using System;
using System.Reflection;

namespace AslHelp.Shared.Extensions;

public static class EnumExtensions
{
    public static TAttribute? GetAttribute<TEnum, TAttribute>(this TEnum value)
        where TEnum : unmanaged, Enum
        where TAttribute : notnull, Attribute
    {
        FieldInfo fieldInfo = typeof(TEnum).GetField(value.ToString());
        return fieldInfo.GetCustomAttribute<TAttribute>(false);
    }
}
