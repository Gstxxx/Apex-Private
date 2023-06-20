using System;
using System.Runtime.InteropServices;

namespace MiHoYo_Sharp.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_HANDLE_INFORMATION
    {
        public ulong HandleCount;
        public SYSTEM_HANDLE Handles;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SYSTEM_HANDLE
    {
        public uint ProcessId;
        public byte ObjectTypeNumber;
        public byte Flags;
        public ushort Handle;
        public IntPtr Object;
        public IntPtr GrantedAccess;
    }
}