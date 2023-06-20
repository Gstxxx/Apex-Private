using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using MiHoYo_Sharp.Enums;

namespace MiHoYo_Sharp.Classes
{
    public class PInvoke
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr CreateFileA([MarshalAs(UnmanagedType.LPStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] FileAccess access, [MarshalAs(UnmanagedType.U4)] FileShare share,
            IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes, IntPtr templateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer,
            int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, IntPtr lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer,
            int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern unsafe bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, void* lpInBuffer,
            uint nInBufferSize, void* lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
            SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, nuint count);

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
            SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, nint count);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, int count);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("ntdll.dll", CharSet = CharSet.Auto)]
        public static extern NTSTATUS NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS systemInformationClass,
            IntPtr systemInformation,
            int systemInformationLength, out int returnLength);

        [DllImport("ntdll.dll")]
        public static extern uint NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass, IntPtr SystemInformation, uint SystemInformationLength, uint ReturnLength);
    }
}