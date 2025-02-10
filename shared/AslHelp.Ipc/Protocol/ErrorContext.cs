using System;
using System.Collections.Generic;
using System.Reflection;

using AslHelp.Shared;
using AslHelp.Shared.Extensions;

namespace AslHelp.Ipc.Protocol;

public abstract class IpcExit<TExitCode>
    where TExitCode : unmanaged, Enum
{
    private class Info(TExitCode code)
    {
        public string Id { get; } = $"{typeof(TExitCode).FullName}.{code}";

        public TExitCode Code { get; } = code;
    }

    private class Info2(TExitCode code, string message) : Info(code)
    {
        public string Message { get; } = message;
    }

    private static readonly Dictionary<TExitCode, string> _messages = [];

    static IpcExit()
    {
        bool hasOk = false;

        foreach (FieldInfo fi in typeof(TExitCode).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            TExitCode value = fi.GetValue<TExitCode>();

            if (fi.GetCustomAttribute<ErrorAttribute>() is { } err)
            {
                _messages.Add(value, err.Message);
            }
            else if (fi.GetCustomAttribute<OkAttribute>() is { })
            {
                if (hasOk)
                {
                    ThrowDuplicateOkAttribute(fi);
                }

                Ok = value;
                hasOk = true;
            }
            else
            {
                throw new ArgumentException($"""
                    '{fi.Name}' must be marked with either {nameof(OkAttribute)} or {nameof(ErrorAttribute)}
                    """);
            }
        }

        if (!hasOk)
        {
            throw new ArgumentException($"No field marked with {nameof(OkAttribute)}.");
        }
    }

    public static TExitCode Ok { get; }

    public static string GetErrorMessage(TExitCode code)
    {
        if (_messages.TryGetValue(code, out string message))
        {
            return message;
        }

        if (!Enum.IsDefined(typeof(TExitCode), code))
        {
            return $"Unknown error code '{code}'.";
        }

        if (code.Equals(Ok))
        {
            return "No error.";
        }
    }

    private static void ThrowDuplicateOkAttribute(FieldInfo fi)
    {
        string fullName = $"{fi.DeclaringType.FullName}.{fi.Name}";
        string msg = $"Only one value may be marked with `{typeof(OkAttribute).FullName}`.";

        ThrowHelper.ThrowArgumentException(fullName, msg);
    }
}

public abstract class IpcExit<TExitCode, TRequest>
    where TExitCode : unmanaged, Enum
    where TRequest : IRequest
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
public sealed class ErrorAttribute : Attribute
{
    public ErrorAttribute(string message)
    {
        ThrowHelper.ThrowIfNull(message);

        Message = message;
    }

    public string Message { get; }
}
