using System;
using System.Collections.Generic;
using System.Reflection;

namespace AslHelp.Ipc.Protocol;

public abstract class IpcExit<TExitCode>
    where TExitCode : unmanaged, Enum
{
    private static readonly Dictionary<TExitCode, string> _messages = [];

    static IpcExit()
    {
        foreach (FieldInfo fi in typeof(TExitCode).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            if (fi.GetCustomAttribute<OkAttribute>() is { })
            {
                Ok = (TExitCode)fi.GetValue(null);
            }
            else if (fi.GetCustomAttribute<ErrorAttribute>() is ErrorAttribute err)
            {
                if (err.Message is null)
                {
                    throw new ArgumentException($"Error message for {fi.Name} cannot be null.");
                }

                _messages.Add((TExitCode)fi.GetValue(null), err.Message);
            }
            else
            {
                throw new ArgumentException($"""
                    '{fi.Name}' must be marked with either {nameof(OkAttribute)} or {nameof(ErrorAttribute)}
                    """);
            }
        }
    }

    public static TExitCode Ok { get; }

    public static string GetErrorMessage(TExitCode code)
    {
        return _messages.TryGetValue(code, out string message)
            ? message
            : $"Unknown error code '{code}'.";
    }
}

public abstract class IpcExit<TExitCode, TRequest>
    where TExitCode : unmanaged, Enum
    where TRequest : IRequest<TExitCode>
{
    private static readonly Dictionary<TExitCode, string> _messages = [];

    static IpcExit()
    {
        foreach (FieldInfo fi in typeof(TExitCode).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            if (fi.GetCustomAttribute<OkAttribute>() is { })
            {
                Ok = (TExitCode)fi.GetValue(null);
            }
            else if (fi.GetCustomAttribute<ErrorAttribute>() is ErrorAttribute err)
            {
                if (err.Message is null)
                {
                    throw new ArgumentException($"Error message for {fi.Name} cannot be null.");
                }

                _messages.Add((TExitCode)fi.GetValue(null), err.Message);
            }
            else
            {
                throw new ArgumentException($"""
                    '{fi.Name}' must be marked with either {nameof(OkAttribute)} or {nameof(ErrorAttribute)}
                    """);
            }
        }
    }

    public static TExitCode Ok { get; }

    public static string GetErrorMessage(TExitCode code, TRequest request)
    {
        return _messages.TryGetValue(code, out string message)
            ? message
            : $"Unknown error code '{code}'.";
    }

    private sealed class MsgInfo
    {

    }
}

[AttributeUsage(AttributeTargets.Field)]
public sealed class OkAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field)]
public sealed class ErrorAttribute(string message) : Attribute
{
    public string Message { get; } = message;
}
