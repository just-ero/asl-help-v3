﻿using System;

namespace AslHelp.Memory.Native.Enums;

/// <summary>
///     Provides standard access rights.
/// </summary>
/// <remarks>
///     For further information see:
///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/secauthz/standard-access-rights">Standard Access Rights</see></i>
/// </remarks>
[Flags]
internal enum StandardAccess : uint
{
    /// <summary>
    ///     Required to delete the object.
    /// </summary>
    DELETE = 0x010000,

    /// <summary>
    ///     Required to read information in the security descriptor for the object.
    /// </summary>
    READ_CONTROL = 0x020000,

    /// <summary>
    ///     Required to modify the DACL in the security descriptor for the object.
    /// </summary>
    WRITE_DAC = 0x040000,

    /// <summary>
    ///     Required to change the owner in the security descriptor for the object.
    /// </summary>
    WRITE_OWNER = 0x080000,

    /// <summary>
    ///     The right to use the object for synchronization.
    /// </summary>
    SYNCHRONIZE = 0x100000
}
