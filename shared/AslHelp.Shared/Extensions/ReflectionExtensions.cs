using System.Reflection;

namespace AslHelp.Shared.Extensions;

public static class ReflectionExtensions
{
    public static T GetValue<T>(this FieldInfo field, object? instance = null)
    {
        return (T)field.GetValue(instance);
    }

    public static void SetValue<T>(this FieldInfo field, T value, object? instance = null)
    {
        field.SetValue(instance, value);
    }
}
