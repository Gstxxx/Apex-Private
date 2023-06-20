using System.Runtime.InteropServices;

namespace Private_Apex.Core.Cheat.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct ViewMatrix
{
    public float[] matrix;
}