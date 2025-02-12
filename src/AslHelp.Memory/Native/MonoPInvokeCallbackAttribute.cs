using System;

namespace AslHelp.Memory.Native;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class MonoPInvokeCallbackAttribute : Attribute
{
    public MonoPInvokeCallbackAttribute(Type delegateType)
    {
        DelegateType = delegateType;
    }

    public Type DelegateType { get; }
}
