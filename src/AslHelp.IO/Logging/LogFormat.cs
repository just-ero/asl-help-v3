using System;

namespace AslHelp.IO.Logging;

[Flags]
public enum LogFormat
{
    None,

    ShowTimestamp = 1 << 0,
    ShowMember = 1 << 1,
    ShowLocation = 1 << 2
}
