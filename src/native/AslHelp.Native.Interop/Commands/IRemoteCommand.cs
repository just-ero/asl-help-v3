namespace AslHelp.Native.Interop.Commands;

public interface IRemoteCommand<out TResult>
    where TResult : unmanaged
{
    string Uid { get; }
}

public interface IRemoteCommand<in T, out TResult>
    where T : unmanaged
    where TResult : unmanaged
{
    string Uid { get; }
}
