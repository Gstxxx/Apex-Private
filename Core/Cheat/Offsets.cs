namespace Private_Apex.Core.Cheat;

public static class Offsets
{
    public static ulong EntityList = 0x1a1e3b8; // (Offsets) cl_entitylist
    public static ulong Name = 0x589; // (Classes - CBaseEntity) m_iName CBaseEntity    m_iName
    public static ulong Team = 0x448; // (Classes - CBaseEntity) m_iTeamNum
    public static ulong VisibleTime = 0x1ad4; // lastVisibleTime
    // player
    public static ulong AbsVelocity = 0x0140; // m_vecAbsVelocity
    public static ulong ViewAngles = 0x2580 - 0x14;//m_ammoPoolCapacity - 0x14
    public static ulong AimPunch = 0x24a0;//m_currentFrameLocalPlayer.m_vecPunchWeapon_Angle
    public static ulong Playerhealth = 0x438; //m_iHealth
    public static ulong PLayerShield = 0x170;//m_shieldHealth
    public static ulong LifeState = 0x798;  //m_lifeState, >0;// dead
    public static ulong BleedoutState = 0x2720;//m_bleedoutState, >0;// knocked


    //aimbot

    //bonepos
    public static ulong Origin = 0x014c; // m_vecAbsOrigin (m_vecAbsOrigin=0x0004) 0x014c
    public static ulong Bones = 0x0f38; // m_bConstrainBetweenEndpoints 
    //main
    public static ulong LocalPlayer = 0x1dcf5e8; // (Offsets) local_player
    public static ulong ViewRender = 0x7544150; // (Offsets) view_render // ViewRender
    public static ulong ViewMatrix = 0x11a210; // (Offsets) view_matrix // ViewMatrix

   
    public static ulong CameraPos = 0x1f40; // camera_origin // CPlayer!camera_origin
    public static ulong iSignifierName = 0x0580;
    public static ulong CustomScriptInt = 0x16b8;
    // Weapons
    public static ulong BulletSpeed = 0x1f40; // m_flProjectileSpeed
    public static ulong BulletScale = 0x1f48; // m_flProjectileScale
    public static ulong ZoomFov = 0x1730 + 0x00b8; // m_playerData + m_curZoomFOV //0x17e8
    // Glow
    public static ulong GlowEnable = 0x3c8; //0x3c8 (Offsets) glow_enable | 1;// enabled, 2;// disabled
    public static ulong GlowThroughWall = 0x3d0; // 2; enabled, 5; disabled
    public static ulong GlowMode = 0x2c0 + 0x4; // (Offsets) glow_type | m_highlightFunctionBits + 0x4
    public static ulong GlowColor = 0x1b8 + 0x18; //0x1b8 (Offsets) glow_color | m_highlightParams + 0x18
    public static ulong GameMode = 0x1E07190;// mp_gamemode
}