using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using AslHelp.Shared;

namespace AslHelp.Native;

internal static partial class Exports
{
    private static void Listen()
    {
        using var client = Udp.Receiver;
        var endpoint = Udp.ReceiverEndpoint;

        Trace.WriteLine("Listening for commands...");

        while (true)
        {
            var message = client.Receive(ref endpoint);
            if (message.Length == 0)
            {
                continue;
            }

            var command = message[0];
            Trace.WriteLine($"Received command: {command}");

            if (command == 0)
            {
                break;
            }
        }
    }

    [UnmanagedCallersOnly(EntryPoint = NativeApi.StartListener)]
    public static unsafe bool StartListener()
    {
        Task.Run(Listen);
        return true;
    }
}
