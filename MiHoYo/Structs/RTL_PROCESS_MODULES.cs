using System.Runtime.InteropServices;

namespace MiHoYo_Sharp.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RTL_PROCESS_MODULE_INFORMATION
    {
        public ulong Handle;
        public ulong MappedBase;
        public ulong ImageBase;
        public uint ImageSize;
        public uint Flags;
        public ushort LoadOrderIndex;
        public ushort InitOrderIndex;
        public ushort LoadCount;
        public ushort OffsetToFileName;
        public unsafe fixed byte FullPathName[256];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RTL_PROCESS_MODULES
    {
        public uint NumberOfModules;
        public RTL_PROCESS_MODULE_INFORMATION Modules;
    }
}