using Newtonsoft.Json;
using Private_Apex.Core.Cheat.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Private_Apex.Core.Cheat
{
    [Serializable]
    [DataContract]
    public class ConfigStruct
    {
        [JsonProperty("AimFov")]
        public float AimFov { get; set; }

        [JsonProperty("AimSmooth")]
        public float AimSmooth { get; set; }

        [JsonProperty("AimDistance")]
        public float AimDistance { get; set; }

        [JsonProperty("AimBone")]
        public int AimBone { get; set; }

        [JsonProperty("Enable Aim")]
        public bool EnableAim { get; set; }

        [JsonProperty("Visible color")]
        public RGBColor visible { get; set; }

        [JsonProperty("Invisible color")]
        public RGBColor invisible { get; set; }

        [JsonProperty("Glow Mode")]
        public  int GlowMode { get; set; }

        [JsonProperty("Enable Glow")]
        public bool Enable_glow { get; set; }
        [JsonProperty("Enable No Recoil")]
        public bool EnableRec { get; set; }



    }

    public class OffsetsJson
    {
        [JsonProperty("EntityList")]
        public ulong EntityList { get; set; }

        [JsonProperty("LocalPlayer")]
        public ulong LocalPlayer { get; set; }

        [JsonProperty("Name")]
        public ulong Name { get; set; }

        [JsonProperty("Team")]
        public ulong Team { get; set; }

        [JsonProperty("VisibleTime")]
        public ulong VisibleTime { get; set; }
        [JsonProperty("Origin")]
        public ulong Origin { get; set; }
        [JsonProperty("Bones")]
        public ulong Bones { get; set; }

        [JsonProperty("AbsVelocity")]
        public ulong AbsVelocity { get; set; }
        [JsonProperty("ViewAngles")]
        public ulong ViewAngles { get; set; }
        [JsonProperty("AimPunch")] 
        public ulong AimPunch { get; set; } 
        [JsonProperty("Playerhealth")]
        public ulong Playerhealth { get; set; }
        [JsonProperty("PLayerShield")]
        public ulong PLayerShield { get; set; } 
        [JsonProperty("LifeState")] 
        public ulong LifeState { get; set; } 
        [JsonProperty("BleedoutState")]
        public ulong BleedoutState { get; set; }

        [JsonProperty("ViewRender")]
        public ulong ViewRender { get; set; } 

        [JsonProperty("ViewMatrix")]
        public ulong ViewMatrix { get; set; }
        [JsonProperty("CameraPos")]
        public ulong CameraPos { get; set; }
        [JsonProperty("iSignifierName")]
        public ulong iSignifierName { get; set; }
        [JsonProperty("CustomScriptInt")]
        public ulong CustomScriptInt { get; set; }
        [JsonProperty("ItemGlow")]
        public ulong ItemGlow { get; set; }

        [JsonProperty("BulletSpeed")]
        public ulong BulletSpeed { get; set; }
        [JsonProperty("BulletScale")]
        public ulong BulletScale { get; set; }

        [JsonProperty("GlowEnable")]
        public ulong GlowEnable { get; set; }
        [JsonProperty("GlowThroughWall")]
        public ulong GlowThroughWall { get; set; }
        [JsonProperty("GlowMode")]
        public ulong GlowMode { get; set; }
        [JsonProperty("GlowColor")]
        public ulong GlowColor { get; set; } 
    }
}