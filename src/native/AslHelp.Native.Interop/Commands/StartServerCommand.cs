namespace AslHelp.Native.Interop.Commands;

public enum StartServerRequest
{
    Unknown,

    StartMonoServer
}

public enum StartServerResponse : uint
{
    Ok,

    RequestUnknown,
    UnhandledException,

    LibraryNotFound
}
