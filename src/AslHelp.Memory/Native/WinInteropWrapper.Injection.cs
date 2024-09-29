using System;
using System.Diagnostics;
using System.IO;

using AslHelp.Memory.Native.Enums;
using AslHelp.Memory.Native.Structs;
using AslHelp.Shared.Results;

namespace AslHelp.Memory.Native;

internal static unsafe partial class WinInteropWrapper
{
    public static bool Inject(this Process process, string dllToInject)
    {
        return Inject((nuint)(nint)process.Handle, (uint)process.Id, dllToInject);
    }

    public static bool Inject(nuint processHandle, uint processId, string dllToInject)
    {
        Module? module = Path.IsPathRooted(dllToInject)
            ? GetModuleHandleFromPath(processId, dllToInject)
            : GetModuleHandleFromName(processId, dllToInject);

        if (module is { Base: > 0 })
        {
            return true;
        }

        if (GetModuleHandleFromName(processId, Lib.Kernel32) is not { Base: > 0 } k32)
        {
            return false;
        }

        SymbolInfo pLoadLibraryW = GetSymbol(processHandle, k32!, "LoadLibraryW");
        if (pLoadLibraryW.Address == 0)
        {
            return false;
        }

        fixed (char* pDllToInject = dllToInject)
        {
            uint length = (uint)((dllToInject.Length + 1) * sizeof(char));
            nuint pModuleName = AllocateRemote(processHandle, pDllToInject, length);

            if (pModuleName == 0)
            {
                return false;
            }

            return CreateRemoteThreadAndGetExitCode(processHandle, (nuint)pLoadLibraryW.Address, pModuleName) != 0;
            // TODO: FreeRemote
        }
    }

    public static uint CallRemoteFunction<T>(this Process process, string moduleName, string functionName, T arg)
        where T : unmanaged
    {
        return CallRemoteFunction((nuint)(nint)process.Handle, (uint)process.Id, moduleName, functionName, arg);
    }

    public static Result<uint> CallRemoteFunction<T>(nuint processHandle, uint processId, string injectedDll, string functionName, T arg)
        where T : unmanaged
    {
        Module? module = Path.IsPathRooted(injectedDll)
            ? GetModuleHandleFromPath(processId, injectedDll)
            : GetModuleHandleFromName(processId, injectedDll);

        if (module is not { Base: > 0 })
        {
            return Result<uint>.Err("Module not found");
        }

        SymbolInfo pFunction = GetSymbol(processHandle, module, functionName);
        if (pFunction.Address == 0)
        {
            return 0;
        }

        nuint pArg = AllocateRemote(processHandle, arg);
        if (pArg == 0)
        {
            return 0;
        }

        return CreateRemoteThreadAndGetExitCode(processHandle, (nuint)pFunction.Address, pArg);
    }

    public static bool Eject(this Process process, string dllToEject)
    {
        return Eject((nuint)(nint)process.Handle, (uint)process.Id, dllToEject);
    }

    public static bool Eject(nuint processHandle, uint processId, string dllToEject)
    {
        Module? module = Path.IsPathRooted(dllToEject)
            ? GetModuleHandleFromPath(processId, dllToEject)
            : GetModuleHandleFromName(processId, dllToEject);

        if (module is not { Base: > 0 })
        {
            return true;
        }

        var k32 = GetModuleHandleFromName(processId, Lib.Kernel32);
        var pFreeLibrary = GetSymbol(processHandle, k32!, "FreeLibrary");

        return CreateRemoteThreadAndGetExitCode(processHandle, (nuint)pFreeLibrary.Address, module.Base) != 0;
    }

    private static nuint AllocateRemote<T>(nuint processHandle, T data)
        where T : unmanaged
    {
        return AllocateRemote(processHandle, &data, (uint)sizeof(T));
    }

    private static nuint AllocateRemote(nuint processHandle, void* data, uint dataSize)
    {
        var pRemote = WinInterop.VirtualAlloc(
            processHandle,
            0,
            dataSize,
            MemoryRangeState.Commit | MemoryRangeState.Reserve,
            MemoryRangeProtect.ExecuteReadWrite);

        if (pRemote == 0)
        {
            return 0;
        }

        if (!WriteMemory(processHandle, pRemote, data, dataSize))
        {
            pRemote = 0;
        }

        WinInterop.VirtualFree(processHandle, pRemote, dataSize, MemoryRangeState.Release);
        return pRemote;
    }

    private static uint CreateRemoteThreadAndGetExitCode(nuint processHandle, nuint startAddress, nuint arg)
    {
        var threadHandle = WinInterop.CreateRemoteThread(processHandle, null, 0, startAddress, (void*)arg, 0, out _);
        WinInterop.WaitForSingleObject(threadHandle, uint.MaxValue);
        WinInterop.GetExitCodeThread(threadHandle, out var result);
        WinInterop.CloseHandle(threadHandle);

        return result;
    }

    private static Module? GetModuleHandleFromName(uint processId, string moduleName)
    {
        foreach (Module module in EnumerateModules(processId))
        {
            if (module.Name.Equals(moduleName, StringComparison.InvariantCultureIgnoreCase))
            {
                return module;
            }
        }

        return default;
    }

    private static Module? GetModuleHandleFromPath(uint processId, string modulePath)
    {
        foreach (Module module in EnumerateModules(processId))
        {
            if (module.FileName.Equals(modulePath, StringComparison.InvariantCultureIgnoreCase))
            {
                return module;
            }
        }

        return default;
    }
}
