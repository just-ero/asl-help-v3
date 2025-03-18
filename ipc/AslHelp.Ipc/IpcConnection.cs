namespace AslHelp.Ipc;

public static class IpcConnection
{
    public const string PipeName = "asl-help-ipc";
    public const string EntryPoint = "AslHelp.Ipc.Native.Exports::StartServer";
}
