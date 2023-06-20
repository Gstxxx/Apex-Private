using System.Numerics;
using System.Runtime.InteropServices;
using MiHoYo_Sharp.Classes;
using static Private_Apex.Core.Cheat.Utils;
using System.Threading;
using System;
namespace Private_Apex.Core.Cheat.Features;
using Private_Apex.Core.Cheat.Structs;

public class Aimbot
{
    public static bool EnableAim = false;
    public static bool EnableRec = false;
    public static float AimFov = 60f;
    public static float AimSmooth = 1f;
    public static float AimDistance = 200f;
    public static int AimBone = 8;

    //Bones:
    //1: Groin
    //2: Waist
    //3: Center Chest
    //4: Right Hand
    //5: Low Neck
    //6, 7: High Neck
    //8: Head
    //9: Heart
    // https://imgur.com/gyLRlux

    public static void MainThread()
    {
        while (true)
        {
            if (!EnableAim)
                return;
            //0x23
            if ((GetAsyncKeyState(0x2) & 0x8000) == 0) // https://docs.microsoft.com/pt-br/windows/win32/inputdev/virtual-key-codes
                continue;

            var entity = GetBestTarget(out var viewMatrix);

            if (entity == 0)
                continue;

            var hitBox = GetBonePos(entity, 8);
            var hitBox2D = new Vector2();

            if (!WorldToScreen(viewMatrix, hitBox, ref hitBox2D))
                continue;

            if (hitBox2D.X != 0 || hitBox2D.Y != 0)
                moveTo(hitBox2D.X, hitBox2D.Y);

            Thread.Sleep(10);
        }
    }
    private static ulong GetBestTarget(out float[] viewMatrix)
    {
        var bestDistance = AimDistance;
        ulong bestTarget = 0;

        var localPlayer = Implementation.Read<ulong>(Program.BaseAddress + Offsets.LocalPlayer);
        var viewRender = Implementation.Read<ulong>(Program.BaseAddress + Offsets.ViewRender);

        viewMatrix = new float[16];

        Implementation.ReadArray(Implementation.Read<ulong>(viewRender + Offsets.ViewMatrix), ref viewMatrix);

        for (var i = 0; i < 100; ++i)
        {
            var entity = GetEntityById(i, Program.BaseAddress);

            if (entity == 0 || entity == localPlayer)
                continue;

            // Check same team
            if (IsTeam(entity, localPlayer))
                continue;

            var hitBox = GetBonePos(entity, 8);
            var hitBox2D = new Vector2();

           // Prediction(entity, ref hitBox);

            //if(EnableRec)
            //NoRecoil(entity);

            if (!WorldToScreen(viewMatrix, hitBox, ref hitBox2D))
                continue;

            hitBox2D.X -= Width / 2;
            hitBox2D.Y -= Height / 2;

            var crosshairDistance = Math.Sqrt(hitBox2D.X * hitBox2D.X + hitBox2D.Y * hitBox2D.Y);

            if (!(crosshairDistance <= bestDistance))
                continue;

            if (crosshairDistance > AimFov)
                continue;

            if (!IsVisible(entity, i))
                continue;

            bestDistance = (float)crosshairDistance;
            bestTarget = entity;
        }

        return bestTarget;
    }

    private static Vector3 oldPunch = Vector3.Zero;

    //private static void NoRecoil(ulong entity)
    //{
    //    if ((GetAsyncKeyState(0x1) & 0x8000) > 0)
    //    {
    //        var mAngle = Implementation.Read<Vector3>(entity + Offsets.ViewAngles) + (oldPunch - Implementation.Read<Vector3>(entity + Offsets.AimPunch));
    //        NormalizeFloat(ref mAngle);

    //        Implementation.Write(entity + Offsets.ViewAngles, mAngle);
    //        oldPunch = Implementation.Read<Vector3>(entity + Offsets.AimPunch);
    //    }
    //}

    private static void Prediction(ulong entity, ref Vector3 hitbox)
    {
        var projecitleSpeed = Implementation.Read<float>(entity + Offsets.BulletSpeed);

        if (projecitleSpeed > 1)
        {
            var time = Math.Abs(DistanceTo(Implementation.Read<Vector3>(entity + Offsets.CameraPos), hitbox) / projecitleSpeed);

            var delta = Implementation.Read<Vector3>(entity + Offsets.AbsVelocity) * time;

            hitbox.X += delta.X;
            hitbox.Y += delta.Y;
            hitbox.Z += delta.Z;
        }
    }

    private static void moveTo(float x, float y)
    {
        var centerX = Width / 2;
        var centerY = Height / 2;

        var xDelta = 0f;
        var yDelta = 0f;

        if (x != 0f)
        {
            if (x > centerX)
            {
                xDelta = -(centerX - x);
                xDelta /= AimSmooth;
                if (xDelta + centerX > centerX * 2f) xDelta = 0f;
            }

            if (x < centerX)
            {
                xDelta = x - centerX;
                xDelta /= AimSmooth;
                if (xDelta + centerX < 0f) xDelta = 0f;
            }
        }

        if (y != 0f)
        {
            if (y > centerY)
            {
                yDelta = -(centerY - y);
                yDelta /= AimSmooth;
                if (yDelta + centerY > centerY * 2f) yDelta = 0f;
            }

            if (y < centerY)
            {
                yDelta = y - centerY;
                yDelta /= AimSmooth;
                if (yDelta + centerY < 0f) yDelta = 0f;
            }
        }
        mouse_event(0x00000001, (int)xDelta, (int)yDelta, 0, 0);
    }

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, uint dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern int GetAsyncKeyState(int vKey);
}