﻿using System;

namespace AslHelp.Memory.Native.Enums;

/// <summary>
///     Provides ToolHelp32 flags.
/// </summary>
/// <remarks>
///     For further information see:
///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot">CreateToolhelp32Snapshot function (tlhelp32.h)</see></i>
/// </remarks>
[Flags]
internal enum ThFlags : uint
{
    /// <summary>
    ///     Includes all heaps of the process specified in the snapshot.
    /// </summary>
    TH32CS_SNAPHEAPLIST = 0x00000001,

    /// <summary>
    ///     Includes all processes in the system in the snapshot.
    /// </summary>
    TH32CS_SNAPPROCESS = 0x00000002,

    /// <summary>
    ///     Includes all threads in the system in the snapshot.
    /// </summary>
    TH32CS_SNAPTHREAD = 0x00000004,

    /// <summary>
    ///     Includes all modules of the process specified in the snapshot.
    /// </summary>
    TH32CS_SNAPMODULE = 0x00000008,

    /// <summary>
    ///     Includes all 32-bit modules of the process specified in the snapshot.
    /// </summary>
    TH32CS_SNAPMODULE32 = 0x00000010,

    /// <summary>
    ///     Indicates that the snapshot handle is to be inheritable.
    /// </summary>
    TH32CS_INHERIT = 0x80000000,

    /// <summary>
    ///     Includes all processes and threads in the system, plus the heaps and modules of the process specified in the snapshot.
    /// </summary>
    TH32CS_SNAPALL = TH32CS_SNAPHEAPLIST | TH32CS_SNAPPROCESS | TH32CS_SNAPTHREAD | TH32CS_SNAPMODULE
}
