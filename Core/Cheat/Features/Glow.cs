using System;
using System.Threading;
using MiHoYo_Sharp.Classes;
using Private_Apex.Core.Cheat.Structs;

using static Private_Apex.Core.Cheat.Utils;

namespace Private_Apex.Core.Cheat.Features;

internal static class Glow
{
    public static RGBColor visible = new RGBColor(155f, 75f, 225f);
    public static RGBColor invisible = new RGBColor(225f, 0f, 0f);
    public static RGBColor ItemGlow = new RGBColor(225f, 255f, 0f);
    public static GlowMode visibleB = new GlowMode()
    {
        GeneralGlowMode = 101,
        BorderGlowMode = 101,
        BorderSize = 50,
        TransparentLevel = 90
    };
    public static GlowMode invisibleB = new GlowMode()
    {
        GeneralGlowMode = 109,
        BorderGlowMode = 109,
        BorderSize = 50,
        TransparentLevel = 25
    };

    public static int GlowMode = 0;
    public static bool Enable_glow = true;
    public static int[] HEIRLOOM = { 1, 22, 38, 64, 165, 169, 170, 226, 227, };
    enum ItemID
    {
        LIGHT_ROUNDS = 124,
        ENERGY_AMMO,
        SHOTGUN_SHELLS,
        HEAVY_ROUNDS,
        SNIPER_AMMO,
        ARROWS,

        ULTIMATE_ACCELERANT = 164,
        PHOENIX_KIT,
        MED_KIT,
        SYRINGE,
        SHIELD_BATTERY,
        SHIELD_CELL,

        HELMET_LV1 = 170,
        HELMET_LV2,
        HELMET_LV3,
        HELMET_LV4,
        BODY_ARMOR_LV1,
        BODY_ARMOR_LV2,
        BODY_ARMOR_LV3,
        BODY_ARMOR_LV4,
        EVO_SHIELD_LV0,
        EVO_SHIELD_LV1,
        EVO_SHIELD_LV2,
        EVO_SHIELD_LV3,
        EVO_SHIELD_LV4,

        KNOCKDOWN_SHIELD_LV1 = 184,
        KNOCKDOWN_SHIELD_LV2,
        KNOCKDOWN_SHIELD_LV3,
        KNOCKDOWN_SHIELD_LV4,
        BACKPACK_LV1,
        BACKPACK_LV2,
        BACKPACK_LV3,
        BACKPACK_LV4,

        THERMITE_GRENADE = 192,
        FRAG_GRENADE,
        ARC_STAR,

        HCOG_CLASSIC = 195,
        HCOG_BRUISER,
        HOLO,
        VARIABLE_HOLO,
        DIGITAL_THREAT,
        HCOG_RANGER,
        VARIABLE_AOG,
        SNIPER,
        VARIABLE_SNIPER,
        DIGITAL_SNIPER_THREAT,

        BARREL_STABILIZER_LV1 = 205,
        BARREL_STABILIZER_LV2,
        BARREL_STABILIZER_LV3,
        BARREL_STABILIZER_LV4,
        LIGHT_MAGAZINE_LV1,
        LIGHT_MAGAZINE_LV2,
        LIGHT_MAGAZINE_LV3,
        LIGHT_MAGAZINE_LV4,
        HEAVY_MAGAZINE_LV1,
        HEAVY_MAGAZINE_LV2,
        HEAVY_MAGAZINE_LV3,
        HEAVY_MAGAZINE_LV4,
        ENERGY_MAGAZINE_LV1,
        ENERGY_MAGAZINE_LV2,
        ENERGY_MAGAZINE_LV3,
        ENERGY_MAGAZINE_LV4,
        SNIPER_MAGAZINE_LV1,
        SNIPER_MAGAZINE_LV2,
        SNIPER_MAGAZINE_LV3,
        SNIPER_MAGAZINE_LV4,
        SHOTGUN_BOLT_LV1,
        SHOTGUN_BOLT_LV2,
        SHOTGUN_BOLT_LV3,
        STANDARD_STOCK_LV1,
        STANDARD_STOCK_LV2,
        STANDARD_STOCK_LV3,
        SNIPER_STOCK_LV1,
        SNIPER_STOCK_LV2,
        SNIPER_STOCK_LV3,

        TURBOCHARGER = 234,
        SKULLPIERCER_RIFLING = 237,
        HAMMERPOINT_ROUNDS = 238,
        ANVIL_RECEIVER = 239,

        DEADEYE_TEMPO = 245,
        QUICKDRAW_HOLSTER = 246,
        SHATTER_CAPS = 247,
        KINETIC_FEEDER = 248,
        BOOSTED_LOADER = 249,
    };
    public static void MainThread()
    {
        while (true)
        {
            if (GlowMode == 0)
            {

                var localPlayer = Implementation.Read<ulong>(Program.BaseAddress + Offsets.LocalPlayer);
                for (var i = 0; i < 100; ++i)
                {
                    var entity = GetEntityById(i, Program.BaseAddress);

                    if (entity == 0)
                        continue;

                    if (IsTeam(entity, localPlayer))
                        continue;

                    Implementation.Write(entity + Offsets.GlowEnable, 1);
                    Implementation.Write(entity + Offsets.GlowThroughWall, 2);
                    Implementation.Write(entity + Offsets.GlowMode, IsVisible(entity, i) ? visibleB : invisibleB);

                    Implementation.Write(entity + Offsets.GlowColor, IsVisible(entity, i) ? visible : invisible);


                    SubtractCooldownFrame(i);
                }

            }
            else if (GlowMode == 1)
            {
                var localPlayer = Implementation.Read<ulong>(Program.BaseAddress + Offsets.LocalPlayer);

                for (var i = 0; i < 100; ++i)
                {
                    var entity = GetEntityById(i, Program.BaseAddress);

                    if (entity == 0)
                        continue;

                    var entityHandle = Implementation.ReadString(entity + Offsets.Name);

                    if (entityHandle is not "player")
                        continue;

                    var bleed = Implementation.Read<int>(entity + Offsets.BleedoutState);
                    var life = Implementation.Read<int>(entity + Offsets.LifeState);

                    // Check same team
                    if (IsTeam(entity, localPlayer))
                        continue;

                    if (bleed != 0 && life != 0)
                    {

                        visible = new RGBColor(200 / 255f, 40 / 255f, 255 / 255f);
                        invisible = new RGBColor(225 / 255f, 41 / 255f, 159 / 255f);
                    }
                    else
                    {
                        var shield = Implementation.Read<int>(entity + Offsets.PLayerShield);
                        var health = Implementation.Read<int>(entity + Offsets.Playerhealth);

                        if (shield > 100)
                        { //Heirloom armor - Red
                            visible = new RGBColor(155, 0, 0);
                            invisible = new RGBColor(255, 0, 0);

                        }
                        else if (shield > 75)
                        { //Purple armor - Purple
                            visible = new RGBColor(100, 0, 150);
                            invisible = new RGBColor(200, 0, 255);
                        }
                        else if (shield > 50)
                        { //Blue armor - Light blue
                            visible = new RGBColor(0, 0, 255);
                            invisible = new RGBColor(0, 0, 255);
                        }
                        else if (shield > 0)
                        { //White armor - White
                            visible = new RGBColor(0, 75, 155);
                            invisible = new RGBColor(0, 156, 255);
                        }
                        else if (health > 50)
                        { //Above 50% HP - Orange
                            visible = new RGBColor(0, 100 / 255f, 0);
                            invisible = new RGBColor(0, 255 / 255f, 0);
                        }
                        else
                        { //Below 50% HP - Light Red
                            visible = new RGBColor(75, 155, 0);
                            invisible = new RGBColor(155, 255, 0);
                        }

                    }

                    if (Enable_glow)
                    {
                        Implementation.Write(entity + Offsets.GlowEnable, 1);
                        Implementation.Write(entity + Offsets.GlowThroughWall, 2);
                    }
                    else
                    {
                        Implementation.Write(entity + Offsets.GlowEnable, 0);
                        Implementation.Write(entity + Offsets.GlowThroughWall, 5);
                    }
                    Implementation.Write(entity + Offsets.GlowMode, IsVisible(entity, i) ? visibleB : invisibleB);

                    Implementation.Write(entity + Offsets.GlowColor,
                        IsVisible(entity, i) ? new RGBColor(visible.R, visible.G, visible.B) : new RGBColor(invisible.R, invisible.G, invisible.B));

                    SubtractCooldownFrame(i);
                }
            }

            Thread.Sleep(10);
        }
    }
}