using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Private_Apex.Core.Cheat.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct GlowMode
{
    public byte GeneralGlowMode;
    public byte BorderGlowMode;
    public byte BorderSize;
    public byte TransparentLevel;
}