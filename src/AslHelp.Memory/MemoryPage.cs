using AslHelp.Memory.Native.Enums;
using AslHelp.Memory.Native.Structs;

namespace AslHelp.Memory;

public readonly struct MemoryRange
{
    public MemoryRange(nuint @base, uint regionSize, MemoryRangeProtect protect, MemoryRangeState state, MemoryRangeType type)
    {
        Base = @base;
        RegionSize = regionSize;
        Protect = protect;
        State = state;
        Type = type;
    }

    internal unsafe MemoryRange(MemoryBasicInformation mbi)
    {
        Base = (nuint)mbi.BaseAddress;
        RegionSize = (uint)mbi.RegionSize;
        Protect = mbi.Protect;
        State = mbi.State;
        Type = mbi.Type;
    }

    public nuint Base { get; }
    public uint RegionSize { get; }

    public MemoryRangeProtect Protect { get; }
    public MemoryRangeState State { get; }
    public MemoryRangeType Type { get; }

    public override string ToString()
    {
        return $"{nameof(MemoryRange)} {{ {nameof(Base)} = 0x{(ulong)Base:X}, {nameof(RegionSize)} = 0x{RegionSize:X} }}";
    }
}
