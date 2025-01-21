using System;

namespace AslHelp.Ipc.Errors;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ErrAttribute(
    string message) : Attribute
{
    public string Message { get; } = message;
}
