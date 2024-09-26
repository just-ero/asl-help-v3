using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using AslHelp.Memory.Native.Enums;
using AslHelp.Memory.Native.Structs;

namespace AslHelp.Memory.Native;

public static unsafe partial class WinInteropWrapper
{
    public static nuint Inject(this Process process, string dllToInject)
    {
        return Inject((nuint)(nint)process.Handle, (uint)process.Id, dllToInject);
    }

    public static nuint Inject(nuint processHandle, uint processId, string dllToInject)
    {
        var handle = Path.IsPathRooted(dllToInject)
            ? GetModuleHandleFromPath(processId, dllToInject)
            : GetModuleHandleFromName(processId, dllToInject);

        if (handle != 0)
        {
            return handle;
        }

        var k32 = WinInterop.GetModuleHandle("kernel32.dll");
        Console.WriteLine($"k32: {k32:X}");
        var pLoadLibrary = WinInterop.GetProcAddress(k32, "LoadLibraryW"u8);
        Console.WriteLine($"pLoadLibrary: {pLoadLibrary:X}");

        var length = (uint)((dllToInject.Length + 1) * sizeof(char));
        Console.WriteLine($"length: {length}");
        var pModuleAlloc = WinInterop.VirtualAlloc(processHandle, 0, length, MemState.MEM_COMMIT | MemState.MEM_RESERVE, MemProtect.PAGE_READWRITE);
        Console.WriteLine($"pModuleAlloc: {pModuleAlloc:X}");

        fixed (char* pDllToInject = dllToInject)
        {
            var success = WriteMemory(processHandle, pModuleAlloc, pDllToInject, length);
            Console.WriteLine($"success: {success}");
        }

        var threadHandle = WinInterop.CreateRemoteThread(processHandle, null, 0, pLoadLibrary, (void*)pModuleAlloc, 0, out _);
        Console.WriteLine($"threadHandle: {threadHandle:X}");
        var @event = WinInterop.WaitForSingleObject(threadHandle, uint.MaxValue);
        Console.WriteLine($"event: {@event}");
        var success2 = WinInterop.GetExitCodeThread(threadHandle, out var dllHandle);
        Console.WriteLine($"success2: {success2}");
        var success3 = WinInterop.CloseHandle(threadHandle);
        Console.WriteLine($"success3: {success3}");

        return dllHandle;
    }

    private static nuint GetModuleHandleFromName(uint processId, string moduleName)
    {
        foreach (var module in EnumerateModulesTh32(processId))
        {
            var name = StringMarshal.CreateStringFromNullTerminated((char*)module.Module, ModuleEntry32.ModuleLength);

            if (name == moduleName)
            {
                return (nuint)module.ModuleBaseAddress;
            }
        }

        return 0;
    }

    private static nuint GetModuleHandleFromPath(uint processId, string modulePath)
    {
        foreach (var module in EnumerateModulesTh32(processId))
        {
            var path = StringMarshal.CreateStringFromNullTerminated((char*)module.ExePath, ModuleEntry32.ExePathLength);

            if (path == modulePath)
            {
                return (nuint)module.ModuleBaseAddress;
            }
        }

        return 0;
    }
}
