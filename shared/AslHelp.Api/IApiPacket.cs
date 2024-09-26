using System.Linq;

namespace AslHelp.Api;

public interface IApiPacket;

public static class ApiPacketExtensions
{
    public static string Format<T>(this T packet)
        where T : IApiPacket
    {
        var name = typeof(T).Name;
        var properties = typeof(T).GetProperties().Select(p => $"{p.Name} = {p.GetValue(packet)}");

        return $"{name} {{ {string.Join(", ", properties)} }}";
    }
}
