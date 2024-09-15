using System.Net;
using System.Net.Sockets;

namespace AslHelp.Shared;

public static class Udp
{
    private const ushort Port = 45183;

    public static UdpClient Sender => new(AddressFamily.InterNetwork);
    public static UdpClient Receiver => new(Port, AddressFamily.InterNetwork);

    public static IPEndPoint SenderEndpoint => new(IPAddress.Loopback, Port);
    public static IPEndPoint ReceiverEndpoint => new(IPAddress.Loopback, 0);
}
