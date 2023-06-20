using System.Runtime.InteropServices;

namespace MiHoYo_Sharp.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MHYPROT_KERNEL_READ_REQUEST
    {
        public MHYPROT_KERNEL_READ_REQUEST_UNION header;
        public uint size;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MHYPROT_KERNEL_READ_REQUEST_UNION
    {
        [FieldOffset(0)] public uint result;
        [FieldOffset(0)] public ulong address;
    }
}