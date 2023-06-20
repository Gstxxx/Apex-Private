using System;
using System.Runtime.InteropServices;

namespace MiHoYo_Sharp.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LIST_ENTRY
    {
        public IntPtr Flink;
        public IntPtr Blink;
    }
}