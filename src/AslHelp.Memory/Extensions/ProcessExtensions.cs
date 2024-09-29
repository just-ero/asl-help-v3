using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using AslHelp.Collections.Extensions;
using AslHelp.Memory.Native;
using AslHelp.Memory.Native.Enums;
using AslHelp.Memory.Native.Structs;
using AslHelp.Shared;

using Mbi = (
    AslHelp.Memory.Native.Enums.MemoryRangeProtect Protect,
    AslHelp.Memory.Native.Enums.MemoryRangeState State,
    AslHelp.Memory.Native.Enums.MemoryRangeType Type);

namespace AslHelp.Memory.Extensions;

public static class ProcessExtensions
{
    public static bool Is64Bit(this Process process)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        if (!WinInterop.IsWow64Process(processHandle, out bool isWow64))
        {
            ThrowHelper.ThrowWin32Exception();
        }

        return Environment.Is64BitOperatingSystem && !isWow64;
    }

    public static unsafe bool ReadMemory(this Process process, nuint address, void* buffer, uint bufferSize)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        return WinInterop.ReadProcessMemory(processHandle, address, buffer, bufferSize, out nuint nRead)
            && nRead == bufferSize;
    }

    public static unsafe bool WriteMemory(this Process process, nuint address, void* data, uint dataSize)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        return WinInterop.WriteProcessMemory(processHandle, address, data, dataSize, out nuint nWritten)
            && nWritten == dataSize;
    }

    public static IEnumerable<Module> GetModules(this Process process)
    {
        uint processId = (uint)process.Id;
        nuint snapshot = WinInterop.CreateToolhelp32Snapshot(processId, ThFlags.Module | ThFlags.Module32);

        try
        {
            ModuleEntry32 me = new() { Size = (uint)Unsafe.SizeOf<ModuleEntry32>() };
            if (!WinInterop.Module32First(snapshot, ref me))
            {
                yield break;
            }

            do
            {
                yield return new(me);
            } while (WinInterop.Module32Next(snapshot, ref me));
        }
        finally
        {
            WinInterop.CloseHandle(snapshot);
        }
    }

    public static unsafe List<Module> GetModulesLongPathSafe(this Process process)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        if (!WinInterop.EnumProcessModules(processHandle, [], 0, out uint bytesNeeded, ListModulesFilter.ListAll)
            || bytesNeeded == 0)
        {
            return [];
        }

        int count = (int)bytesNeeded / sizeof(nuint);
        nuint[]? mhRented = null;
        Span<nuint> moduleHandles =
            count <= 128
            ? stackalloc nuint[128]
            : (mhRented = ArrayPool<nuint>.Shared.Rent(count));

        if (!WinInterop.EnumProcessModules(processHandle, moduleHandles, (uint)(count * sizeof(nuint)), out bytesNeeded, ListModulesFilter.ListAll))
        {
            ArrayPool<nuint>.Shared.ReturnIfNotNull(mhRented);
            return [];
        }

        List<Module> modules = new(count);

        char[] fnRented;
        Span<char> fileName = fnRented = ArrayPool<char>.Shared.Rent(WinInterop.UnicodeStringMaxChars);

        foreach (nuint moduleHandle in moduleHandles)
        {
            uint length = WinInterop.GetModuleFileName(moduleHandle, fileName);
            if (length == 0)
            {
                continue;
            }

            if (!WinInterop.GetModuleInformation(processHandle, moduleHandle, out ModuleInfo moduleInfo))
            {
                continue;
            }

            Module module = new(moduleInfo, fileName[..(int)length].ToString());
            modules.Add(module);
        }

        ArrayPool<nuint>.Shared.ReturnIfNotNull(mhRented);
        ArrayPool<char>.Shared.Return(fnRented);

        return modules;
    }

    public static IEnumerable<MemoryPage> GetMemoryPages(this Process process, Func<Mbi, bool>? filter = null)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        nuint address = 0x10000, max = (nuint)(process.Is64Bit() ? 0x7FFFFFFEFFFF : 0x7FFEFFFF);

        do
        {
            if (WinInterop.VirtualQuery(processHandle, address, out MemoryBasicInformation mbi) == 0)
            {
                break;
            }

            address += mbi.RegionSize;

            if (filter is not null)
            {
                if (filter((mbi.Protect, mbi.State, mbi.Type)))
                {
                    yield return new(mbi);
                }
            }
            else
            {
                // Default filter.
                if (mbi.State != MemoryRangeState.Commit)
                {
                    continue;
                }

                if ((mbi.Protect & MemoryRangeProtect.Guard) != 0)
                {
                    continue;
                }

                if (mbi.Type != MemoryRangeType.Private)
                {
                    continue;
                }

                yield return new(mbi);
            }
        } while (address < max);
    }
}
