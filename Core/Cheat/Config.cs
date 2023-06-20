using Newtonsoft.Json;
using Private_Apex.Core.Cheat.Features;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Private_Apex.Core.Cheat;

public static class Config
{
    public static ConfigStruct ConfigStructWrapper = new ConfigStruct();

    public static void LoadConfiguration()
    {
        try
        {
            ConfigStructWrapper = JsonConvert.DeserializeObject<ConfigStruct>(File.ReadAllText("Config.json"));

            Aimbot.AimFov = ConfigStructWrapper.AimFov;
            Aimbot.AimSmooth = ConfigStructWrapper.AimSmooth;
            Aimbot.AimDistance = ConfigStructWrapper.AimDistance;
            Aimbot.AimBone = ConfigStructWrapper.AimBone;
            Aimbot.EnableAim = ConfigStructWrapper.EnableAim;
            Aimbot.EnableRec = ConfigStructWrapper.EnableRec;
            

            Glow.visible = ConfigStructWrapper.visible;
            Glow.invisible = ConfigStructWrapper.invisible;
            Glow.GlowMode = ConfigStructWrapper.GlowMode;
            Glow.Enable_glow = ConfigStructWrapper.Enable_glow;
            Console.WriteLine("\nConfig Loaded\n");
        }
        catch { }
    }
    public static void SaveConfiguration()
    {
        try
        {
            ConfigStruct config = new ConfigStruct()
            {
                AimFov = Aimbot.AimFov,
                AimSmooth = Aimbot.AimSmooth,
                AimDistance = Aimbot.AimDistance,
                AimBone = Aimbot.AimBone,
                EnableAim = Aimbot.EnableAim,
                EnableRec = Aimbot.EnableRec,

                
                visible = Glow.visible,
                invisible = Glow.invisible,
                GlowMode = Glow.GlowMode,
                Enable_glow = Glow.Enable_glow,
            };
            File.WriteAllText("Config.json", JsonConvert.SerializeObject(config));
        }
        catch { }
    }

    
    //public static OffsetsJson OffsetsJsonWrap = new OffsetsJson();
    //public static void LoadOffsets()
    //{
    //    try
    //    {
    //        OffsetsJsonWrap = JsonConvert.DeserializeObject<OffsetsJson>(File.ReadAllText("Offsets.json"));
    //        Offsets.EntityList = OffsetsJsonWrap.EntityList;
    //        Offsets.LocalPlayer = OffsetsJsonWrap.LocalPlayer;
    //        Offsets.Name = OffsetsJsonWrap.Name;
    //        Offsets.Team = OffsetsJsonWrap.Team;
    //        Offsets.VisibleTime = OffsetsJsonWrap.VisibleTime;
    //        Offsets.Origin = OffsetsJsonWrap.Origin;
    //        Offsets.Bones = OffsetsJsonWrap.Bones;
    //        Offsets.AbsVelocity = OffsetsJsonWrap.AbsVelocity;
    //        Offsets.ViewAngles = OffsetsJsonWrap.ViewAngles;
    //        Offsets.AimPunch = OffsetsJsonWrap.AimPunch;
    //        Offsets.Playerhealth = OffsetsJsonWrap.Playerhealth;
    //        Offsets.PLayerShield = OffsetsJsonWrap.PLayerShield;
    //        Offsets.LifeState = OffsetsJsonWrap.LifeState;
    //        Offsets.BleedoutState = OffsetsJsonWrap.BleedoutState;
    //        Offsets.ViewRender = OffsetsJsonWrap.ViewRender;
    //        Offsets.ViewMatrix = OffsetsJsonWrap.ViewMatrix;
    //        Offsets.CameraPos = OffsetsJsonWrap.CameraPos;
    //        Offsets.iSignifierName = OffsetsJsonWrap.iSignifierName;
    //        Offsets.CustomScriptInt = OffsetsJsonWrap.CustomScriptInt;
    //        Offsets.ItemGlow = OffsetsJsonWrap.ItemGlow;
    //        Offsets.BulletSpeed = OffsetsJsonWrap.BulletSpeed;
    //        Offsets.BulletScale = OffsetsJsonWrap.BulletScale;
    //        Offsets.GlowEnable = OffsetsJsonWrap.GlowEnable;
    //        Offsets.GlowThroughWall = OffsetsJsonWrap.GlowThroughWall;
    //        Offsets.GlowMode = OffsetsJsonWrap.GlowMode;
    //        Offsets.GlowColor = OffsetsJsonWrap.GlowColor;
    //        Console.WriteLine("\nOffsets Loaded\n");
    //    }
    //    catch { }
    //}
    //public static void SaveOffsets()
    //{
    //    try
    //    {
    //        OffsetsJson offsets = new OffsetsJson()
    //        {
    //            EntityList = Offsets.EntityList,
    //            LocalPlayer = Offsets.LocalPlayer,
    //            Name = Offsets.Name,
    //            Team = Offsets.Team,
    //            VisibleTime = Offsets.VisibleTime,
    //            Origin = Offsets.Origin,
    //            Bones = Offsets.Bones,
    //            AbsVelocity = Offsets.AbsVelocity,
    //            ViewAngles = Offsets.ViewAngles,
    //            AimPunch = Offsets.AimPunch,
    //            Playerhealth = Offsets.Playerhealth,
    //            PLayerShield = Offsets.PLayerShield,
    //            LifeState = Offsets.LifeState,
    //            BleedoutState = Offsets.BleedoutState, //BleedoutState;
    //            ViewRender = Offsets.ViewRender, //ViewRender;
    //            ViewMatrix = Offsets.ViewMatrix, //ViewMatrix;
    //            CameraPos = Offsets.CameraPos, //CameraPos;
    //            iSignifierName = Offsets.iSignifierName, //iSignifierName;
    //            CustomScriptInt = Offsets.CustomScriptInt, //CustomScriptInt;
    //            ItemGlow = Offsets.ItemGlow, //ItemGlow;
    //            BulletSpeed = Offsets.BulletSpeed, //BulletSpeed;
    //            BulletScale = Offsets.BulletScale, //BulletScale;
    //            GlowEnable = Offsets.GlowEnable, //GlowEnable;
    //            GlowThroughWall = Offsets.GlowThroughWall, //GlowThroughWall;
    //            GlowMode = Offsets.GlowMode, //GlowMode;
    //            GlowColor = Offsets.GlowColor, //GlowColor;
    //        };
    //        File.WriteAllText("Offsets.json", JsonConvert.SerializeObject(offsets));
    //    }
    //    catch { }
    //}
}