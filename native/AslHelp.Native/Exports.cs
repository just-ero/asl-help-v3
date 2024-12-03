using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AslHelp.Api;
using AslHelp.Api.Servers;
using AslHelp.Native.Mono;

namespace AslHelp.Native;

internal static partial class Exports
{
    [UnmanagedCallersOnly(EntryPoint = ApiResourceStrings.ApiEntryPoint)]
    public static unsafe bool ApiEntryPoint(int connectionType)
    {
        Task.Run(connectionType switch
        {
            _ => ServerConnection<NativeMonoServer>
        });

        return true;
    }

    private static void ServerConnection<TServer>()
        where TServer : BaseServer, new()
    {
        using TServer server = new();

        try
        {
            Output.Log("Waiting for connection...");
            server.WaitForConnection();
            Output.Log("  => Connected!");

            while (server.IsConnected)
            {
                Output.Log("Processing next request...");
                server.ProcessNextRequest();
            }

            Output.Log("  => Disconnected.");
        }
        catch (Exception ex)
        {
            Output.Log($"Main loop encountered an exception: {ex}");
        }
    }
}
