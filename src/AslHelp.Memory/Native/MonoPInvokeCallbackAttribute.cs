using System;

namespace AslHelp.Memory.Native;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class MonoPInvokeCallbackAttribute : Attribute
{
    public Type DelegateType { get; }

    public MonoPInvokeCallbackAttribute(Type delegateType)
    {
        DelegateType = delegateType;
    }
}
