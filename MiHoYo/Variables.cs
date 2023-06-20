namespace MiHoYo_Sharp
{
    public static class Variables
    {
        public static string MHYPROT_SERVICE_NAME = "mhyprot2";
        public static string MHYPROT_DISPLAY_NAME = "mhyprot2";
        public static string MHYPROT_SYSFILE_NAME = "mhyprot.sys";
        public static string MHYPROT_SYSMODULE_NAME = "mhyprot2.sys";

        public static string MHYPROT_DEVICE_NAME = "\\\\?\\\\mhyprot2";

        public static uint MHYPROT_IOCTL_INITIALIZE = 0x80034000;
        public static uint MHYPROT_IOCTL_READ_KERNEL_MEMORY = 0x83064000;
        public static uint MHYPROT_IOCTL_READ_WRITE_USER_MEMORY = 0x81074000;

        public static uint MHYPROT_OFFSET_SEEDMAP = 0xA0E8;
    }
}