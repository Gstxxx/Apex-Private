using System;
using System.IO;
using System.Runtime.InteropServices;
using Private_Apex.Properties;
using MiHoYo_Sharp.Enums;
using MiHoYo_Sharp.Structs;
using static MiHoYo_Sharp.Classes.PInvoke;

namespace MiHoYo_Sharp.Classes
{
    public static class Mhyprot
    {
        private static IntPtr _hDriver = IntPtr.Zero;
        private static ulong[] _seedmap = new ulong[312];

        public static bool Init()
        {
            Console.WriteLine(@"Initializing vulnerable driver...");
            Unload();
            // place the driver binary into the temp path
            //
            var placementPath = Path.GetTempPath() + Variables.MHYPROT_SYSFILE_NAME;

            if (File.Exists(placementPath))
                File.Delete(placementPath);

            //
            // create driver sys from memory
            //
            File.WriteAllBytes(placementPath, Resources.mhyprot2);
            Console.WriteLine(@"Preparing Service...");

            //
            // create and start service using winapi, this needs administrator privileage
            //
            ServiceInstaller.InstallAndStart(Variables.MHYPROT_SERVICE_NAME, Variables.MHYPROT_DISPLAY_NAME,
                placementPath);
            Console.WriteLine(@"Service is up!");

            //
            // open the handle of its driver device
            //
            _hDriver = CreateFileA(Variables.MHYPROT_DEVICE_NAME, FileAccess.ReadWrite, FileShare.None, IntPtr.Zero,
                FileMode.Open, 0, IntPtr.Zero);
            if (_hDriver == (IntPtr)(-1))
                return false;

            Console.WriteLine(@"mhyprot initialized successfully!");
            return true;
        }

        public static void Unload()
        {
            try
            {
                if (_hDriver != (IntPtr)(-1) && _hDriver != IntPtr.Zero)
                    CloseHandle(_hDriver);
                ServiceInstaller.StopService(Variables.MHYPROT_SERVICE_NAME);
                ServiceInstaller.Uninstall(Variables.MHYPROT_SERVICE_NAME);
            }
            catch
            {
                // ignored
            }
        }
        private static unsafe bool RequestIoctl(uint ioctlCode, IntPtr inBuffer, int inBufferSize)
        {
            var outBuffer = Marshal.AllocHGlobal(inBufferSize);
            var outBufferSize = 0;

            var reference = __makeref(outBufferSize);

            var result = DeviceIoControl(_hDriver, ioctlCode, inBuffer, inBufferSize, outBuffer, inBufferSize,
                *(IntPtr*)&reference, IntPtr.Zero);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (outBufferSize != 0)
                memcpy(inBuffer, outBuffer, inBufferSize);

            Marshal.FreeHGlobal(outBuffer);

            return result;
        }

        public static unsafe bool DriverInit(bool debugPrints, bool printSeeds)
        {
            //
            // the driver initializer
            //
            Console.WriteLine(@"Initializing Driver...");
            var initializer = new MHYPROT_INITIALIZE
            {
                _m_002 = 0x0BAEBAEEC,
                _m_003 = 0x0EBBAAEF4FFF89042
            };
            var reference = __makeref(initializer);
            if (!RequestIoctl(Variables.MHYPROT_IOCTL_INITIALIZE, *(IntPtr*)&reference, Marshal.SizeOf(initializer)))
                Console.WriteLine(@"Failed to initialize mhyplot driver implementation");

            //
            // driver's base address in the system
            //

            var mhyprotAddress = WinUtils.obtain_sysmodule_address(Variables.MHYPROT_SYSFILE_NAME);
            if (mhyprotAddress == 0)
            {
                Console.Write(@"Failed to locate mhyprot module address");
                return false;
            }

            //
            // read the pointer that points to the seedmap that used to encrypt payloads
            // the pointer on the [driver.sys + 0xA0E8]
            //
            var seedmapAddress = read_kernel_memory<ulong>(mhyprotAddress + Variables.MHYPROT_OFFSET_SEEDMAP);

            //
            // read the entire seedmap as size of 0x9C0
            //
            if (!read_kernel_memory(seedmapAddress, ref _seedmap))
            {
                Console.WriteLine(@"[!] failed to pickup seedmap from kernel\n");
                return false;
            }

            Console.WriteLine(@"Driver Intiailized with Successfully!");
            return true;
        }

        private static unsafe bool read_kernel_memory<T>(ulong address, ref T[] array) where T : struct
        {
            var size = (int)((ulong)Marshal.SizeOf(array[0]) * (ulong)array.Length);
            var typedReference = __makeref(array[0]);
            read_kernel_memory(address, *(IntPtr*)&typedReference, size);
            return true;
        }

        private static unsafe T read_kernel_memory<T>(ulong address) where T : struct
        {
            T buffer = default;
            var reference = __makeref(buffer);
            read_kernel_memory(address, *(IntPtr*)&reference, Marshal.SizeOf<T>());
            return buffer;
        }

        public static unsafe bool read_kernel_memory(ulong address, IntPtr buffer, int size)
        {
            var payloadSize = size + sizeof(int);

            var payload = new MHYPROT_KERNEL_READ_REQUEST();

            payload.header.address = address;
            payload.size = (uint)size;

            var payloadMemory = CopyStructToMemory(payload);

            var outBuffer = Marshal.AllocHGlobal(payloadSize);
            var outBufferSize = 0;

            var reference2 = __makeref(outBufferSize);

            _ = DeviceIoControl(_hDriver, Variables.MHYPROT_IOCTL_READ_KERNEL_MEMORY, payloadMemory,
                payloadSize, outBuffer, payloadSize, *(IntPtr*)&reference2, IntPtr.Zero);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (outBufferSize == 0) return false;

            payload = Marshal.PtrToStructure<MHYPROT_KERNEL_READ_REQUEST>(outBuffer);

            if (payload.header.result == 0)
            {
                memcpy(buffer, outBuffer + 4, size);
                Marshal.FreeHGlobal(outBuffer);
                return true;
            }

            Marshal.FreeHGlobal(outBuffer);

            return false;
        }


        public static unsafe bool ReadUserMemory(int pid, ulong address, IntPtr buffer, int size)
        {
            var payload = new MHYPROT_USER_READ_WRITE_REQUEST
            {
                action = (uint)MHYPROT_ACTION.Read,
                process_id = (uint)pid,
                address = (IntPtr)address,
                buffer = buffer,
                size = (uint)size

                //unknown_00 = 0xCCCCCCCC,
                //unknown_01 = 0xCCCCCCCC,
                //unknown_02 = 0xCCCCCCCC,
                //random_key = 0xCCCCCCCCCCCCCCCC,
            };

            encrypt_payload(&payload, Marshal.SizeOf(payload));
            var payloadReference = __makeref(payload);

            var res = RequestIoctl(Variables.MHYPROT_IOCTL_READ_WRITE_USER_MEMORY, *(IntPtr*)&payloadReference,
                Marshal.SizeOf(payload));
            return res;
        }

        public static unsafe bool WriteUserMemory(int pid, ulong address, IntPtr buffer, int size)
        {
            var payload = new MHYPROT_USER_READ_WRITE_REQUEST
            {
                action = (uint)MHYPROT_ACTION.Write,
                process_id = (uint)pid,
                address = buffer,
                buffer = (IntPtr)address,
                size = (uint)size

                //unknown_00 = 0xCCCCCCCC,
                //unknown_01 = 0xCCCCCCCC,
                //unknown_02 = 0xCCCCCCCC,
                //random_key = 0xCCCCCCCCCCCCCCCC,
            };

            encrypt_payload(&payload, Marshal.SizeOf(payload));
            var payloadReference = __makeref(payload);

            var res = RequestIoctl(Variables.MHYPROT_IOCTL_READ_WRITE_USER_MEMORY, *(IntPtr*)&payloadReference,
                Marshal.SizeOf(payload));
            return res;
        }

        private static unsafe void encrypt_payload(MHYPROT_USER_READ_WRITE_REQUEST* payload, int size)
        {
            if (size % 8 != 0)
            {
                Console.WriteLine(@"[!] Payload size is not aligned to 8 bytes");
                return;
            }

            if (size / 8 >= 312)
            {
                Console.WriteLine(@"[!] Payload size must be less than 0x9C0");
                return;
            }

            var pointerPayload = (ulong*)payload;
            ulong keyToBase = 0;

            for (uint i = 1; i < size / 8; i++)
            {
                var key = GenerateKey(_seedmap[i - 1]);
                pointerPayload[i] = pointerPayload[i] ^ key ^ keyToBase ^ pointerPayload[0];
                keyToBase += 0x10;
            }
        }


        private static ulong GenerateKey(ulong seed)
        {
            var k = (((((seed >> 29) & 0x555555555) ^ seed) & 0x38EB3FFFF6D3) << 17) ^ ((seed >> 29) & 0x555555555) ^
                    seed;
            return ((k & 0xFFFFFFFFFFFFBF77u) << 37) ^ k ^ ((((k & 0xFFFFFFFFFFFFBF77u) << 37) ^ k) >> 43);
        }

        private static IntPtr CopyStructToMemory<T>(T obj) where T : struct
        {
            var unmanagedAddress = AllocEmptyStruct<T>();
            try
            {
                Marshal.StructureToPtr(obj, unmanagedAddress, true);

                return unmanagedAddress;
            }
            finally
            {
                Marshal.FreeHGlobal(unmanagedAddress);
            }
        }

        private static IntPtr AllocEmptyStruct<T>() where T : struct
        {
            var structSize = Marshal.SizeOf<T>();
            var structPointer = AllocZeroFilled(structSize);

            return structPointer;
        }

        private static IntPtr AllocZeroFilled(int size)
        {
            var allocatedPointer = Marshal.AllocHGlobal(size);
            ZeroMemory(allocatedPointer, size);

            return allocatedPointer;
        }

        private static void ZeroMemory(IntPtr pointer, int size)
        {
            for (var i = 0; i < size; ++i) Marshal.WriteByte(pointer + i, 0x0);
        }
    }
}