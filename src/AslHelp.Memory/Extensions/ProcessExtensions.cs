using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using AslHelp.Collections.Extensions;
using AslHelp.Memory.Errors;
using AslHelp.Memory.Native;
using AslHelp.Memory.Native.Enums;
using AslHelp.Memory.Native.Structs;
using AslHelp.Shared;
using AslHelp.Shared.Results;
using AslHelp.Shared.Results.Errors;

using Mbi = (
    AslHelp.Memory.Native.Enums.MemoryRangeProtect Protect,
    AslHelp.Memory.Native.Enums.MemoryRangeState State,
    AslHelp.Memory.Native.Enums.MemoryRangeType Type);

namespace AslHelp.Memory.Extensions;

public static class ProcessExtensions
{
    public static Result<bool> Is64Bit(this Process process)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        if (!WinInterop.IsWow64Process(processHandle, out bool isWow64))
        {
            return MemoryError.FromLastWin32Error();
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

    public static unsafe Result<List<Module>> GetModulesLongPathSafe(this Process process)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        if (!WinInterop.EnumProcessModules(processHandle, [], 0, out uint bytesNeeded, ListModulesFilter.ListAll)
            || bytesNeeded == 0)
        {
            return MemoryError.FromLastWin32Error();
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
            return MemoryError.FromLastWin32Error();
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
        if (!process.Is64Bit()
            .TryUnwrap(out bool is64Bit, out _))
        {
            yield break;
        }

        nuint processHandle = (nuint)(nint)process.Handle;
        nuint address = 0x10000, max = (nuint)(is64Bit ? 0x7FFFFFFEFFFF : 0x7FFEFFFF);

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

    public static bool Inject(this Process process, string dllToInject)
    {
        Result<Module> rModule = process.GetModulesLongPathSafe()
            .AndThen<Module>(modules =>
            {
                return Path.IsPathRooted(dllToInject)
                    ? modules.FirstOrDefault(m => m.FileName.Equals(dllToInject, StringComparison.InvariantCultureIgnoreCase))
                    : modules.FirstOrDefault(m => m.Name.Equals(dllToInject, StringComparison.InvariantCultureIgnoreCase));
            });

        if (rModule.IsOk)
        {
            return true;
        }

        if (!process.Is64Bit()
            .TryUnwrap(out bool is64Bit, out _))
        {
            return false;
        }

        if (is64Bit)
        {
            return Inject64(process, dllToInject);
        }
        else
        {
            return Inject32(process, dllToInject);
        }
    }

    private static unsafe bool Inject32(this Process process, string dllToInject)
    {
        Result<DebugSymbol> rLoadLibraryW = process.GetModulesLongPathSafe()
            .AndThen<Module>(
                modules => modules.FirstOrDefault(m => m.Name.Equals(Lib.Kernel32, StringComparison.InvariantCultureIgnoreCase)))
            .AndThen(m => m.GetSymbol(process, "LoadLibraryW"));

        if (!rLoadLibraryW
            .TryUnwrap(out DebugSymbol loadLibraryW, out _))
        {
            return false;
        }

        fixed (char* pDllToInject = dllToInject)
        {
            if (!process.AllocateString(dllToInject)
                .TryUnwrap(out nuint pDll, out _))
            {
                return false;
            }

            if (process.CreateRemoteThreadAndGetExitCode(loadLibraryW.Address, pDll)
                .TryUnwrap(out uint exitCode, out _))
            {
                return exitCode != 0;
            }

            process.Free(pDll);

            return false;
        }
    }

    private static unsafe bool Inject64(this Process process, string dllToInject)
    {
        nuint kernel32 = WinInterop.GetModuleHandle(Lib.Kernel32);
        if (kernel32 == 0)
        {
            return false;
        }

        nuint loadLibrary = WinInterop.GetProcAddress(kernel32, "LoadLibraryW"u8);
        if (loadLibrary == 0)
        {
            WinInterop.CloseHandle(kernel32);
            return false;
        }

        fixed (char* pDllToInject = dllToInject)
        {
            if (!process.AllocateString(dllToInject)
                .TryUnwrap(out nuint pDll, out _))
            {
                WinInterop.CloseHandle(kernel32);
                return false;
            }

            if (process.CreateRemoteThreadAndGetExitCode(loadLibrary, pDll)
                .TryUnwrap(out uint exitCode, out _))
            {
                return exitCode != 0;
            }

            process.Free(pDll);

            return false;
        }
    }

    public static unsafe Result<uint> CreateRemoteThreadAndGetExitCode(this Process process, nuint startAddress, nuint arg)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        nuint threadHandle = WinInterop.CreateRemoteThread(processHandle, null, 0, startAddress, (void*)arg, 0, out _);
        if (threadHandle == 0)
        {
            return MemoryError.FromLastWin32Error();
        }

        if (WinInterop.WaitForSingleObject(threadHandle, uint.MaxValue) != 0)
        {
            return MemoryError.FromLastWin32Error();
        }

        if (!WinInterop.GetExitCodeThread(threadHandle, out uint result))
        {
            return MemoryError.FromLastWin32Error();
        }

        WinInterop.CloseHandle(threadHandle);

        return result;
    }

    public static unsafe Result<nuint> Allocate<T>(this Process process, T data)
        where T : unmanaged
    {
        return process.Allocate(&data, (uint)sizeof(T));
    }

    public static unsafe Result<nuint> Allocate(this Process process, void* data, uint dataSize)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        nuint pRemote = WinInterop.VirtualAlloc(
            processHandle,
            0,
            dataSize,
            MemoryRangeState.Commit | MemoryRangeState.Reserve,
            MemoryRangeProtect.ExecuteReadWrite);

        if (pRemote == 0)
        {
            return MemoryError.FromLastWin32Error();
        }

        if (!process.WriteMemory(pRemote, data, dataSize))
        {
            return MemoryError.FromLastWin32Error();
        }

        return pRemote;
    }

    public static unsafe Result<nuint> AllocateString(this Process process, string value)
    {
        fixed (char* pValue = value)
        {
            uint length = (uint)((value.Length + 1) * sizeof(char));
            return process.Allocate(pValue, length);
        }
    }

    public static unsafe Result<nuint> AllocateString(this Process process, ReadOnlySpan<byte> value)
    {
        fixed (byte* pValue = value)
        {
            return process.Allocate(pValue, (uint)value.Length);
        }
    }

    public static bool Free(this Process process, nuint address)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        return WinInterop.VirtualFree(processHandle, address, 0, MemoryRangeState.Release);
    }
}
