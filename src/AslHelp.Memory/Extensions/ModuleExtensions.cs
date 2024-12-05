using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using AslHelp.Memory.Errors;
using AslHelp.Memory.Native;
using AslHelp.Memory.Native.Enums;
using AslHelp.Memory.Native.Structs;
using AslHelp.Memory.Utils;
using AslHelp.Shared.Results;
using AslHelp.Shared.Results.Errors;

using Mbi = (
    AslHelp.Memory.Native.Enums.MemoryRangeProtect Protect,
    AslHelp.Memory.Native.Enums.MemoryRangeState State,
    AslHelp.Memory.Native.Enums.MemoryRangeType Type);

namespace AslHelp.Memory.Extensions;

public static class ModuleExtensions
{
    public static IEnumerable<MemoryPage> GetMemoryPages(this Module module, Func<Mbi, bool>? filter = null)
    {
        nuint processHandle = (nuint)(nint)module.Parent.Handle;
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

    public static unsafe Result<DebugSymbol> GetSymbol(this Module module, string symbolName, string? pdbDirectory = null)
    {
        nuint processHandle = (nuint)(nint)module.Parent.Handle;
        if (!WinInterop.SymInitialize(processHandle, pdbDirectory, false))
        {
            return MemoryError.FromLastWin32Error();
        }

        nuint symLoadBase = WinInterop.SymLoadModule(processHandle, 0, module.FileName, null, module.Base, module.MemorySize, null, 0);

        if (symLoadBase == 0)
        {
            return MemoryError.FromLastWin32Error();
        }

        if (!WinInterop.SymFromName(processHandle, symbolName, out SymbolInfo symbol))
        {
            return MemoryError.FromLastWin32Error();
        }

        _ = WinInterop.SymCleanup(processHandle);

        return new DebugSymbol(symbol);
    }

    public static unsafe Result<List<DebugSymbol>> GetSymbols(this Module module, string? symbolMask = "*", string? pdbDirectory = null)
    {
        nuint processHandle = (nuint)(nint)module.Parent.Handle;

        var callback =
            (delegate* unmanaged[Stdcall]<SymbolInfo*, uint, void*, int>)Marshal.GetFunctionPointerForDelegate(_enumSymbolsCallbackDelegate);

        List<DebugSymbol> symbols = [];
        void* pSymbols = Unsafe.AsPointer(ref symbols);

        if (!WinInterop.SymInitialize(processHandle, pdbDirectory, false))
        {
            return MemoryError.FromLastWin32Error();
        }

        nuint symLoadBase = WinInterop.SymLoadModule(processHandle, 0, module.FileName, null, module.Base, module.MemorySize, null, 0);
        if (symLoadBase == 0)
        {
            WinInterop.SymCleanup(processHandle);
            return MemoryError.FromLastWin32Error();
        }

        if (!WinInterop.SymEnumSymbols(processHandle, symLoadBase, symbolMask, callback, pSymbols))
        {
            WinInterop.SymCleanup(processHandle);
            return MemoryError.FromLastWin32Error();
        }

        WinInterop.SymCleanup(processHandle);

        return symbols;
    }

    private static readonly unsafe WinInterop.PsymEnumeratesymbolsCallback _enumSymbolsCallbackDelegate = EnumSymbolsCallback;

    [MonoPInvokeCallback(typeof(WinInterop.PsymEnumeratesymbolsCallback))]
    private static unsafe int EnumSymbolsCallback(SymbolInfo* pSymInfo, uint symbolSize, void* userContext)
    {
        Unsafe.AsRef<List<DebugSymbol>>(userContext).Add(new(*pSymInfo));

        return 1;
    }

    public static Result Eject(this Module module)
    {
        return module.Parent.GetModule(Lib.Kernel32)
            .AndThen(kernel32 => kernel32.GetSymbol("FreeLibrary"))
            .AndThen(sym => module.Parent.CreateRemoteThreadAndGetExitCode(sym.Address, module.Base))
            .AndThen(exitCode => exitCode != 0
                ? Result.Ok()
                : MemoryError.FromLastWin32Error());
    }

    public static Result<uint> CallRemoteFunction<T>(this Module module, string functionName, T arg)
        where T : unmanaged
    {
        return module.Parent.Is64Bit()
            .AndThen(is64Bit => is64Bit
                ? module.CallRemoteFunction64(functionName, arg)
                : module.CallRemoteFunction32(functionName, arg));
    }

    private static unsafe Result<uint> CallRemoteFunction32<T>(this Module module, string functionName, T arg)
        where T : unmanaged
    {
        if (!module.GetSymbol(functionName)
            .TryUnwrap(out DebugSymbol function, out IResultError? err))
        {
            return Result<uint>.Err(err);
        }

        if (!module.Parent.Allocate(&arg, (uint)sizeof(T))
            .TryUnwrap(out nuint pArg, out err))
        {
            return Result<uint>.Err(err);
        }

        Result<uint> result = module.Parent.CreateRemoteThreadAndGetExitCode(function.Address, pArg);

        module.Parent.Free(pArg);
        return result;
    }

    private static unsafe Result<uint> CallRemoteFunction64<T>(this Module module, string functionName, T arg)
        where T : unmanaged
    {
        nuint pFunction = WinInterop.GetProcAddress(module.Base, Encoding.UTF8.GetBytes(functionName));
        if (pFunction == 0)
        {
            return MemoryError.FromLastWin32Error();
        }

        if (!module.Parent.Allocate(&arg, (uint)sizeof(T))
            .TryUnwrap(out nuint pArg, out IResultError? err))
        {
            return Result<uint>.Err(err);
        }

        Result<uint> result = module.Parent.CreateRemoteThreadAndGetExitCode(pFunction, pArg);

        module.Parent.Free(pArg);
        return result;
    }
}
