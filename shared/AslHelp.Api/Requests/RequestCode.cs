namespace AslHelp.Api.Requests;

public enum RequestCode : byte
{
    Unknown,

    GetMonoImage,
    GetMonoClass,
    GetMonoClassFields,

    Close
}
