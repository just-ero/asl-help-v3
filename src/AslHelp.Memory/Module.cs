using System.Diagnostics;
using System.IO;

using AslHelp.Memory.Native.Structs;
using AslHelp.Memory.Utils;

namespace AslHelp.Memory;

public sealed class Module
{
    public Module(string name, string fileName, nuint @base, uint memorySize)
    {
        Name = name;
        FileName = fileName;
        Base = @base;
        MemorySize = memorySize;
    }

    internal unsafe Module(ModuleEntry32 me)
    {
        Name = StringMarshal.CreateStringFromNullTerminated((char*)me.Module, ModuleEntry32.ModuleLength);
        FileName = StringMarshal.CreateStringFromNullTerminated((char*)me.ExePath, ModuleEntry32.ExePathLength);
        Base = (nuint)me.ModuleBaseAddress;
        MemorySize = me.ModuleBaseSize;
    }

    internal unsafe Module(ModuleInfo mi, string fileName)
    {
        Name = Path.GetFileName(fileName);
        FileName = fileName;
        Base = (nuint)mi.BaseOfDll;
        MemorySize = mi.SizeOfImage;
    }

    public required Process Parent { get; init; }

    public string Name { get; }
    public string FileName { get; }
    public nuint Base { get; }
    public uint MemorySize { get; }

    public FileVersionInfo VersionInfo => FileVersionInfo.GetVersionInfo(FileName);

    public override string ToString()
    {
        return $"{nameof(Module)} {{ {nameof(Name)} = {Name}, {nameof(FileName)} = {FileName}, {nameof(Base)} = 0x{(ulong)Base:X}, {nameof(MemorySize)} = 0x{MemorySize:X} }}";
    }
}
