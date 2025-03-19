namespace AslHelp.Interop.Commands;

public readonly struct StartServer
    : IRemoteCommand<StartServer.Command, StartServer.ExitCode>
{
    public const string Uid = $"{SR.ExportPrefix}.{nameof(StartServer)}";

    public enum Command
    {
        Unknown,

        StartMonoServer
    }

    public enum ExitCode : uint
    {
        Ok,

        RequestUnknown,
        UnhandledException,

        MonoLibraryNotFound
    }

    string IRemoteCommand<Command, ExitCode>.Uid => Uid;
}
