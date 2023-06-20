using System;
using System.Runtime.InteropServices;

namespace MiHoYo_Sharp.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MHYPROT_USER_READ_WRITE_REQUEST
    {
        public ulong random_key;
        public uint action;
        public uint unknown_00;
        public uint process_id;
        public uint unknown_01;
        public IntPtr buffer;
        public IntPtr address;
        public uint size;
        public uint unknown_02;
    }
}