using System;

namespace AslHelp.Api.Errors;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class ApiErrorMessageAttribute : Attribute
{
    public ApiErrorMessageAttribute(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
