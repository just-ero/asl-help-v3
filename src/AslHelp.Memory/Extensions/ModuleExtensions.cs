using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using AslHelp.Memory.Errors;
using AslHelp.Memory.Native;
using AslHelp.Memory.Native.Enums;
using AslHelp.Memory.Native.Structs;
using AslHelp.Shared.Results;
using AslHelp.Shared.Results.Errors;

using Mbi = (
    AslHelp.Memory.Native.Enums.MemoryRangeProtect Protect,
    AslHelp.Memory.Native.Enums.MemoryRangeState State,
    AslHelp.Memory.Native.Enums.MemoryRangeType Type);

namespace AslHelp.Memory.Extensions;

public static class ModuleExtensions
{
    public static IEnumerable<MemoryPage> GetMemoryPages(this Module module, Process process, Func<Mbi, bool>? filter = null)
    {
        nuint processHandle = (nuint)(nint)process.Handle;
        nuint address = module.Base, max = module.Base + module.MemorySize;

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

    public static unsafe Result<DebugSymbol> GetSymbol(this Module module, Process process, string symbolName, string? pdbDirectory = null)
    {
        nuint processHandle = (nuint)(nint)process.Handle;

        if (!WinInterop.SymInitialize(processHandle, pdbDirectory, false))
        {
            return new Win32Exception();
        }

        nuint symLoadBase = WinInterop.SymLoadModule(processHandle, 0, module.FileName, null, module.Base, module.MemorySize, null, 0);

        if (symLoadBase == 0)
        {
            return new Win32Exception();
        }

        if (!WinInterop.SymFromName(processHandle, symbolName, out SymbolInfo symbol))
        {
            return new Win32Exception();
        }

        _ = WinInterop.SymCleanup(processHandle);

        return new DebugSymbol(symbol);
    }

    public static unsafe Result<List<DebugSymbol>> GetSymbols(this Module module, Process process, string? symbolMask = "*", string? pdbDirectory = null)
    {
        nuint processHandle = (nuint)(nint)process.Handle;

        var callback =
            (delegate* unmanaged[Stdcall]<SymbolInfo*, uint, void*, int>)Marshal.GetFunctionPointerForDelegate(enumSymbolsCallback);

        List<DebugSymbol> symbols = [];
        void* pSymbols = Unsafe.AsPointer(ref symbols);

        if (!WinInterop.SymInitialize(processHandle, pdbDirectory, false))
        {
            return new Win32Exception();
        }

        nuint symLoadBase = WinInterop.SymLoadModule(processHandle, 0, module.FileName, null, module.Base, module.MemorySize, null, 0);
        if (symLoadBase == 0)
        {
            WinInterop.SymCleanup(processHandle);
            return new Win32Exception();
        }

        if (!WinInterop.SymEnumSymbols(processHandle, symLoadBase, symbolMask, callback, pSymbols))
        {
            WinInterop.SymCleanup(processHandle);
            return new Win32Exception();
        }

        WinInterop.SymCleanup(processHandle);

        return symbols;

        static int enumSymbolsCallback(SymbolInfo* pSymInfo, uint symbolSize, void* userContext)
        {
            Unsafe.AsRef<List<DebugSymbol>>(userContext).Add(new(*pSymInfo));

            return 1;
        }
    }

    public static Result<uint> CallRemoteFunction<T>(this Module module, Process process, string functionName, T arg)
        where T : unmanaged
    {
        if (!process.Is64Bit()
            .TryUnwrap(out bool is64Bit, out IResultError? err))
        {
            return Result<uint>.Err(err);
        }

        if (is64Bit)
        {
            return module.CallRemoteFunction64(process, functionName, arg);
        }
        else
        {
            return module.CallRemoteFunction32(process, functionName, arg);
        }
    }

    private static unsafe Result<uint> CallRemoteFunction32<T>(this Module module, Process process, string functionName, T arg)
        where T : unmanaged
    {
        if (!module.GetSymbol(process, functionName)
            .TryUnwrap(out DebugSymbol function, out IResultError? err))
        {
            return Result<uint>.Err(err);
        }

        if (!process.Allocate(&arg, (uint)sizeof(T))
            .TryUnwrap(out nuint pArg, out err))
        {
            return Result<uint>.Err(err);
        }

        Result<uint> result = process.CreateRemoteThreadAndGetExitCode(function.Address, pArg);

        process.Free(pArg);
        return result;
    }

    private static unsafe Result<uint> CallRemoteFunction64<T>(this Module module, Process process, string functionName, T arg)
        where T : unmanaged
    {
        nuint pFunction = WinInterop.GetProcAddress(module.Base, Encoding.UTF8.GetBytes(functionName));
        if (pFunction == 0)
        {
            return MemoryError.FromLastWin32Error();
        }

        if (!process.Allocate(&arg, (uint)sizeof(T))
            .TryUnwrap(out nuint pArg, out IResultError? err))
        {
            return Result<uint>.Err(err);
        }

        Result<uint> result = process.CreateRemoteThreadAndGetExitCode(pFunction, pArg);

        process.Free(pArg);
        return result;
    }
}
