using System;
using System.IO;

using AslHelp.Api.Extensions;

namespace AslHelp.Api.Requests;

/// <summary>
///     Represents a request to load a mono image.
/// </summary>
/// <param name="imageName">
///     The name of the image to load.
/// </param>
public sealed class MonoImageRequest(
    string imageName) : IApiPacket<MonoImageRequest>
{
    [Obsolete("For serialization purposes only.", true)]
    public MonoImageRequest()
        : this(null!) { }

    /// <summary>
    ///     Gets the name of the image to load.
    /// </summary>
    public string ImageName { get; private set; } = imageName;

    void IApiPacket<MonoImageRequest>.Write(Stream stream)
    {
        stream.WriteString(ImageName);
    }

    MonoImageRequest IApiPacket<MonoImageRequest>.Read(Stream stream)
    {
        ImageName = stream.ReadString();

        return this;
    }
}
