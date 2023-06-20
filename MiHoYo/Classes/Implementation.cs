using System;
using System.Runtime.InteropServices;
using System.Text;
using MiHoYo_Sharp.Enums;
using MiHoYo_Sharp.Structs;
using static MiHoYo_Sharp.Classes.PInvoke;

namespace MiHoYo_Sharp.Classes
{
    public static class Implementation
    {
        private static uint EPImageFileName;
        private static uint EPUniqueProcessId;
        private static uint EPSectionBaseAddress;
        private static uint EPActiveProcessLinks;

        private static int _pid;

        private static void FixOffsets()
        {
            var dwBuildNumber = Marshal.ReadInt32((IntPtr)0x7FFE0260);
            switch (dwBuildNumber) // some offsets might be wrong, check it yourself it if does not work
            {
                case 22000:
                    EPImageFileName = 0x5a8;
                    EPUniqueProcessId = 0x440;
                    EPSectionBaseAddress = 0x520;
                    EPActiveProcessLinks = 0x448;
                    break;
                case 19044:
                    EPImageFileName = 0x5a8;
                    EPUniqueProcessId = 0x440;
                    EPSectionBaseAddress = 0x520;
                    EPActiveProcessLinks = 0x448;
                    break;
                case 19043: //WIN10_21H1
                    EPImageFileName = 0x5a8;
                    EPUniqueProcessId = 0x440;
                    EPSectionBaseAddress = 0x520;
                    EPActiveProcessLinks = 0x448;
                    break;
                case 19042: //WIN10_20H2
                    EPImageFileName = 0x5a8;
                    EPUniqueProcessId = 0x440;
                    EPSectionBaseAddress = 0x520;
                    EPActiveProcessLinks = 0x448;
                    break;
                case 19041: //WIN10_20H1
                    EPImageFileName = 0x5a8;
                    EPUniqueProcessId = 0x440;
                    EPSectionBaseAddress = 0x520;
                    EPActiveProcessLinks = 0x448;
                    break;
                case 18363: //WIN10_19H2
                    EPImageFileName = 0x450;
                    EPUniqueProcessId = 0x2e8;
                    EPSectionBaseAddress = 0x3c8;
                    EPActiveProcessLinks = 0x2f0;
                    break;
                case 18362: //WIN10_19H1
                    EPImageFileName = 0x450;
                    EPUniqueProcessId = 0x2e8;
                    EPSectionBaseAddress = 0x3c8;
                    EPActiveProcessLinks = 0x2f0;
                    break;
                case 17763: //WIN10_RS5
                    EPImageFileName = 0x450;
                    EPUniqueProcessId = 0x2e0;
                    EPSectionBaseAddress = 0x3c0;
                    EPActiveProcessLinks = 0x2e8;
                    break;
                case 17134: //WIN10_RS4
                    EPImageFileName = 0x450;
                    EPUniqueProcessId = 0x2e0;
                    EPSectionBaseAddress = 0x3c0;
                    EPActiveProcessLinks = 0x2e8;
                    break;
                case 16299: //WIN10_RS3
                    EPImageFileName = 0x450;
                    EPUniqueProcessId = 0x2e0;
                    EPSectionBaseAddress = 0x3c0;
                    EPActiveProcessLinks = 0x2e8;
                    break;
                case 15063: //WIN10_RS2
                    EPImageFileName = 0x450;
                    EPUniqueProcessId = 0x2e0;
                    EPSectionBaseAddress = 0x3c0;
                    EPActiveProcessLinks = 0x2e8;
                    break;
                case 14393: //WIN10_RS1
                    EPImageFileName = 0x450;
                    EPUniqueProcessId = 0x2e8;
                    EPSectionBaseAddress = 0x3c0;
                    EPActiveProcessLinks = 0x2f0;
                    break;
                case 10586: //WIN10_TH2
                    EPImageFileName = 0x450;
                    EPUniqueProcessId = 0x2e8;
                    EPSectionBaseAddress = 0x3c0;
                    EPActiveProcessLinks = 0x2f0;
                    break;
                default:
                    // check https://www.vergiliusproject.com/kernels/x64/Windows%2011/Insider%20Preview%20(Jun%202021)/_EPROCESS to update the code
                    Console.WriteLine("                            [X] Windows Version no Supported!!,\n\n", dwBuildNumber);
                    Console.WriteLine("                            [X] Supported Version's: 22000 | 21H1 | 20H2 | 20H1 | 19H2 | 19H1 | RS5 | RS4 | RS3 | RS2 | RS1 | TH2,\n\n", dwBuildNumber);
                    break;
            }
        }

        public static unsafe ulong GetProcessBase(int pid)
        {
            FixOffsets();
            ulong _base = 0;
            var _baseReference = __makeref(_base);
            ReadVirtual(GetEProcess(pid) + EPSectionBaseAddress, *(IntPtr*)&_baseReference, Marshal.SizeOf(_base));
            return _base;
        }

        public static unsafe bool Write<T>(ulong address, T value) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var bufferReference = __makeref(value);
            return Mhyprot.WriteUserMemory(_pid, address, *(IntPtr*)&bufferReference, size);
        }

        public static unsafe T Read<T>(ulong address) where T : struct
        {
            T buffer = default;
            var bufferReference = __makeref(buffer);
            var size = Marshal.SizeOf<T>();

            _ = Mhyprot.ReadUserMemory(_pid, address, *(IntPtr*)&bufferReference, size);
            return buffer;
        }
        public static string ReadString(ulong address, int Size)
        {
            var StringBytes = new byte[Size];
            _ = ReadArray(address, ref StringBytes);
            return Encoding.Default.GetString(StringBytes);
        }

        public static string ReadString(ulong address)
        {
            var str = ReadString(address, 255);
            if (str.Contains("\0"))
                str = str.Substring(0, str.IndexOf('\0'));
            return str;
        }

        public static unsafe bool ReadArray<T>(ulong address, ref T[] Array) where T : struct
        {
            var size = (int)((ulong)Marshal.SizeOf(Array[0]) * (ulong)Array.Length);
            var typedReference = __makeref(Array[0]);
            _ = Mhyprot.ReadUserMemory(_pid, address, *(IntPtr*)&typedReference, size);
            return true;
        }

        public static void SetPid(int pid)
        {
            _pid = pid;
        }

        private static SYSTEM_HANDLE_INFORMATION QueryInfo(SYSTEM_INFORMATION_CLASS sysClass)
        {
            var size = Marshal.SizeOf<RTL_PROCESS_MODULES>() + 0x1000;
            var status = NTSTATUS.InfoLengthMismatch;
            var buffer = Marshal.AllocHGlobal(size);
            try
            {
                for (; NTSTATUS.InfoLengthMismatch == status; size *= 2)
                {
                    status = NtQuerySystemInformation(sysClass, buffer, size, out _);
                    if (status == NTSTATUS.Success) continue;
                    Marshal.FreeHGlobal(buffer);
                    buffer = Marshal.AllocHGlobal(size * 2);
                }

                var ret = Marshal.PtrToStructure<SYSTEM_HANDLE_INFORMATION>(buffer);
                return ret;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        private static unsafe IntPtr SFGetEProcess(int pid)
        {
            var handleInfo = QueryInfo(SYSTEM_INFORMATION_CLASS.SystemHandleInformation);
            if (handleInfo.HandleCount == 0)
                return IntPtr.Zero;
            var handleInfoPointer = &handleInfo;
            var handles = (SYSTEM_HANDLE*)((ulong)handleInfoPointer + 8);
            for (ulong i = 0; i < handleInfo.HandleCount; ++i)
            {
                if (handles == null) continue;
                var handle = handles[i];
                if (handle.ProcessId == pid && handle.ObjectTypeNumber == 0x7)
                    return handle.Object;
            }

            return IntPtr.Zero;
        }

        private static unsafe void GetNextProcess(ref LIST_ENTRY a, ulong address)
        {
            var aReference = __makeref(a);
            ReadVirtual(address, *(IntPtr*)&aReference, Marshal.SizeOf(a));
        }

        private static unsafe ulong GetEProcess(int pid)
        {
            var activeProcessLinks = new LIST_ENTRY();
            var activeProcessLinksReference = __makeref(activeProcessLinks);
            ReadVirtual((ulong)(SFGetEProcess(4) + (int)EPActiveProcessLinks).ToInt64(), *(IntPtr*)&activeProcessLinksReference, Marshal.SizeOf(activeProcessLinks));
            while (true)
            {
                ulong nextPid = 0;
                var nextPidReference = __makeref(nextPid);
                var buffer = new byte[0xFFFF];
                var bufferReference = __makeref(buffer[0]);
                var nextLink = (ulong)activeProcessLinks.Flink;
                var next = nextLink - EPActiveProcessLinks;
                ReadVirtual(next + EPUniqueProcessId, *(IntPtr*)&nextPidReference, Marshal.SizeOf(nextPid));
                ReadVirtual(next + EPImageFileName, *(IntPtr*)&bufferReference, buffer.Length);
                GetNextProcess(ref activeProcessLinks, next + EPActiveProcessLinks);

                if (nextPid == (ulong)pid)
                    return next;
                if (nextPid is 4 or 0)
                    return 0;
            }
        }

        private static bool ReadVirtual(ulong address, IntPtr Buffer, int size)
        {
            return Mhyprot.read_kernel_memory(address, Buffer, size);
        }

        private static bool ReadVirtual(IntPtr address, IntPtr Buffer, int size)
        {
            return Mhyprot.read_kernel_memory((ulong)address, Buffer, size);
        }
    }
}