using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Private_Apex.Core.Cheat.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct RGBColor
{
    public float R;
    public float G;
    public float B;

    public RGBColor(float R, float G, float B)
    {
        this.R = R;
        this.G = G;
        this.B = B;
    }
}