using MiHoYo_Sharp.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Private_Apex.Core.Cheat.Structs;

namespace Private_Apex.Core.Cheat;

internal static class Utils
{
    private static readonly float[] oldVisibleTime = new float[100];
    private static readonly int[] visCooldownTime = new int[100];

    public static int Width = GetSystemMetrics(0);
    public static int Height = GetSystemMetrics(1);

    public static float NormalizeFloat(ref Vector3 angle)
    {
        float l = angle.Length();
        if (l != 0f)
            angle /= 1;
        else
            angle.X = angle.Y = angle.Z = 0f;

        return l;
    }

    public static float DistanceTo(Vector3 start, Vector3 end)
    {
        var delta = new Vector3();
        delta.X = start.X - end.X;
        delta.Y = start.Y - end.Y;
        delta.Z = start.Z - end.Z;
        return delta.Length();
    }

    public static Vector3 GetBonePos(ulong player, int id)
    {
        var originPos = Implementation.Read<Vector3>(player + Offsets.Origin);
        var bonesArray = Implementation.Read<ulong>(player + Offsets.Bones);
        var bonePos = new Vector3();

        bonePos.X = Implementation.Read<float>(bonesArray + (ulong)0xCC + (ulong)(id * 0x30)) + originPos.X;
        bonePos.Y = Implementation.Read<float>(bonesArray + (ulong)0xDC + (ulong)(id * 0x30)) + originPos.Y;
        bonePos.Z = Implementation.Read<float>(bonesArray + (ulong)0xEC + (ulong)(id * 0x30)) + originPos.Z;

        return bonePos;
    }

    public static bool WorldToScreen(float[] viewMatrix, Vector3 world, ref Vector2 screen)
    {
        var screenWidth = viewMatrix[12] * world.X + viewMatrix[13] * world.Y + viewMatrix[14] * world.Z +
                          viewMatrix[15];
        if (screenWidth < 0.01f)
            return false;

        screen.X =
            (int)(viewMatrix[0] * world.X + viewMatrix[1] * world.Y + viewMatrix[2] * world.Z + viewMatrix[3]);
        screen.Y =
            (int)(viewMatrix[4] * world.X + viewMatrix[5] * world.Y + viewMatrix[6] * world.Z + viewMatrix[7]);

        var invw = 1f / screenWidth;
        screen.X *= invw;
        screen.Y *= invw;

        var x = Width / 2f;
        var y = Height / 2f;

        x += 0.5f * screen.X * Width + 0.5f;
        y -= 0.5f * screen.Y * Height + 0.5f;

        screen.X = x;
        screen.Y = y;

        return true;
    }

    public static bool IsVisible(ulong entity, int index)
    {
        var entityNewVisibleTime = Implementation.Read<float>(entity + Offsets.VisibleTime);
        if (entityNewVisibleTime != oldVisibleTime[index])
        {
            visCooldownTime[index] = 32;
            oldVisibleTime[index] = entityNewVisibleTime;
            return true;
        }
        else if (visCooldownTime[index] <= 0)
            return false;

        return false;
    }

    public static void SubtractCooldownFrame(int index)
    {
        if (visCooldownTime[index] >= 0)
            visCooldownTime[index] -= 1;
    }

    public static bool IsTeam(ulong ent1, ulong ent2)
    {
        var gameModePtr = Implementation.Read<ulong>(Program.BaseAddress + Offsets.GameMode + 0x58);
        var gameMode = Implementation.ReadString(gameModePtr);

        if (gameMode == "control")
            return (GetTeam(ent1) & 1) == (GetTeam(ent2) & 1);
        else
            return GetTeam(ent1) == GetTeam(ent2);
    }

    public static int GetTeam(ulong entity)
    {
        return Implementation.Read<int>(entity + Offsets.Team);
    }

    public static ulong GetEntityById(int entity, ulong baseAddress)
    {
        var entityAddress = baseAddress + Offsets.EntityList;
        var baseEntity = Implementation.Read<ulong>(entityAddress);
        return baseEntity == 0 ? 0 : Implementation.Read<ulong>(entityAddress + (ulong)(entity << 5));
    }
    public static string GetSignifier(ulong ent)
    {
        ulong sigAddr = Implementation.Read<ulong>(ent + Offsets.iSignifierName);
        return Implementation.ReadString(sigAddr, 0xF);
    }

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);
}