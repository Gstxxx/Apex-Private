using System;
using System.Runtime.InteropServices;

namespace MiHoYo_Sharp.Structs
{
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public unsafe struct SYSTEM_MODULE_INFORMATION
    {
        /// DWORD->unsigned int
        public uint reserved1;

        /// DWORD->unsigned int
        public uint reserved2;

        /// DWORD->unsigned int
        public uint reserved3;

        /// DWORD->unsigned int
        public uint reserved4;

        /// PVOID->void*
        public UIntPtr Base;

        /// ULONG->unsigned int
        public uint Size;

        /// ULONG->unsigned int
        public uint Flags;

        /// USHORT->unsigned short
        public ushort Index;

        /// USHORT->unsigned short
        public ushort NameLength;

        /// USHORT->unsigned short
        public ushort LoadCount;

        /// USHORT->unsigned short
        public ushort ModuleNameOffset;

        /// CHAR[256]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ImageName;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct MODULE_LIST
    {
        /// DWORD->unsigned int
        public uint ModuleCount;


        /// SYSTEM_MODULE_INFORMATION[1]
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public SYSTEM_MODULE_INFORMATION Modules;
    }
}