using System.Linq;

namespace AslHelp.Api;

public interface IApiPacket;

public static class ApiPacketExtensions
{
    public static string Format<T>(this T packet)
        where T : IApiPacket
    {
        string name = typeof(T).Name;
        System.Collections.Generic.IEnumerable<string> properties = typeof(T).GetProperties().Select(p => $"{p.Name} = {p.GetValue(packet)}");

        return $"{name} {{ {string.Join(", ", properties)} }}";
    }
}
