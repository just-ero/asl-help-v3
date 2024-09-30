namespace AslHelp.Api.Requests;

public enum RequestCode : byte
{
    Unknown,

    EnumerableContinue,
    EnumerableBreak,

    GetMonoImage,
    GetMonoClass,
    GetMonoClassFields,

    Close
}
