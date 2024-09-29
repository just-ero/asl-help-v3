using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AslHelp.Memory.Native;
using AslHelp.Memory.Native.Enums;
using AslHelp.Memory.Native.Structs;
using AslHelp.Shared;

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

    public static unsafe DebugSymbol GetSymbol(this Module module, Process process, string symbolName, string? pdbDirectory = null)
    {
        nuint processHandle = (nuint)(nint)process.Handle;

        if (!WinInterop.SymInitialize(processHandle, pdbDirectory, false))
        {
            ThrowHelper.ThrowWin32Exception();
        }

        nuint symLoadBase = WinInterop.SymLoadModule(processHandle, 0, module.FileName, null, module.Base, module.MemorySize, null, 0);

        if (symLoadBase == 0)
        {
            ThrowHelper.ThrowWin32Exception();
        }

        if (!WinInterop.SymFromName(processHandle, symbolName, out SymbolInfo symbol))
        {
            ThrowHelper.ThrowWin32Exception();
        }

        _ = WinInterop.SymCleanup(processHandle);

        return new(symbol);
    }

    public static unsafe List<DebugSymbol> GetSymbols(this Module module, Process process, string? symbolMask = "*", string? pdbDirectory = null)
    {
        nuint processHandle = (nuint)(nint)process.Handle;

        var callback =
            (delegate* unmanaged[Stdcall]<SymbolInfo*, uint, void*, int>)Marshal.GetFunctionPointerForDelegate(enumSymbolsCallback);

        List<DebugSymbol> symbols = [];
        void* pSymbols = Unsafe.AsPointer(ref symbols);

        if (!WinInterop.SymInitialize(processHandle, pdbDirectory, false))
        {
            ThrowHelper.ThrowWin32Exception();
        }

        nuint symLoadBase = WinInterop.SymLoadModule(processHandle, 0, module.FileName, null, module.Base, module.MemorySize, null, 0);
        if (symLoadBase == 0)
        {
            ThrowHelper.ThrowWin32Exception();
        }

        if (!WinInterop.SymEnumSymbols(processHandle, symLoadBase, symbolMask, callback, pSymbols))
        {
            ThrowHelper.ThrowWin32Exception();
        }

        _ = WinInterop.SymCleanup(processHandle);

        return symbols;

        static int enumSymbolsCallback(SymbolInfo* pSymInfo, uint symbolSize, void* userContext)
        {
            Unsafe.AsRef<List<DebugSymbol>>(userContext).Add(new(*pSymInfo));

            return 1;
        }
    }
}
