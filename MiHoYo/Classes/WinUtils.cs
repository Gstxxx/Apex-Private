using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiHoYo_Sharp.Enums;
using MiHoYo_Sharp.Structs;
using static MiHoYo_Sharp.Classes.PInvoke;

namespace MiHoYo_Sharp.Classes
{
    public static class WinUtils
    {
        public static unsafe ulong obtain_sysmodule_address(string target_module_name)
        {
            uint status = 0;
            var buffer = IntPtr.Zero;
            uint alloc_size = 0x10000;

            do
            {
                buffer = Marshal.AllocHGlobal(0x10000);
                uint ReturnLength = 0;

                status = NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemModuleInformation, buffer, alloc_size, ReturnLength);
                if (status != 0)
                {
                    Console.WriteLine(@"Failed to query system module information");
                    Marshal.FreeHGlobal(buffer);
                    return 0;
                }

                if (status == 0xC0000004L)
                {
                    Marshal.AllocHGlobal(buffer);
                    buffer = IntPtr.Zero;
                    alloc_size *= 2;
                }
            } while (status == 0xC0000004L);

            //MODULE_LIST module_information = (MODULE_LIST)Marshal.PtrToStructure(buffer, typeof(MODULE_LIST));
            var modules = new List<SYSTEM_MODULE_INFORMATION>();
            var leng = Marshal.ReadInt32(buffer);
            for (var i = 0; i < leng; ++i)
            {
                var info = (SYSTEM_MODULE_INFORMATION)Marshal.PtrToStructure(
                    buffer + 8 + i * Marshal.SizeOf(typeof(SYSTEM_MODULE_INFORMATION)),
                    typeof(SYSTEM_MODULE_INFORMATION));
                modules.Add(info);
            }

            for (var i = 0; i < leng; ++i)
            {
                var module_entry = modules[i];
                var module_address = module_entry.Base;
                if (module_address.ToUInt64() < 0x8000000000000000) continue;

                var module_name = module_entry.ImageName.Substring(module_entry.ModuleNameOffset);
                if (target_module_name == module_name || module_name.Contains("mhyprot"))
                {
                    Marshal.FreeHGlobal(buffer);
                    return module_address.ToUInt64();
                }
            }

            Marshal.FreeHGlobal(buffer);
            return 0;
        }
    }
}