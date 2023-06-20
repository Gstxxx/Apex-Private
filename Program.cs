using Microsoft.Win32.SafeHandles;
using MiHoYo_Sharp.Classes;
using Newtonsoft.Json;
using Private_Apex.Core;
using Private_Apex.Core.Cheat;
using Private_Apex.Core.Cheat.Features;
using Private_Apex.Properties;
using Private_Apex.Translator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Management;

namespace Private_Apex
{
    class Program
    {
        public static ulong BaseAddress { get; set; }

        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static void WriteCentered(string text)
        {
            Console.CursorLeft = 25;
            Console.WriteLine(text);
        }

        private static void Write(string text, string tag, ConsoleColor color, bool Write)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"({DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}) [");
            Console.ForegroundColor = color;
            Console.Write(tag);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            if (!Write)
                Console.Write(text);
            else
                Console.WriteLine(text);
        }
        public static void WriteInfo(string text, bool WriteLine = true) => Write(text, "i", ConsoleColor.DarkBlue, WriteLine);
        public static void WriteSuccess(string text, bool WriteLine = true) => Write(text, "+", ConsoleColor.Green, WriteLine);
        public static void WriteError(string text, bool WriteLine = true) => Write(text, "x", ConsoleColor.Red, WriteLine);
        public static bool Confirm(string title)
        {
            ConsoleKey response;
            do
            {
                WriteInfo($"{ title } [y/n] ");
                response = Console.ReadKey(false).Key;
                if (response != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return (response == ConsoleKey.Y);
        }

        [Serializable]
        [JsonObject]
        public class IniStruct
        {
            [JsonProperty("Login")]
            public string Login { get; set; }

            [JsonProperty("Password")]
            public string Password { get; set; }
        }

        public static string GetIPAddress()
        {
            string IPADDRESS = new WebClient().DownloadString($"http://ip-api.com/line/?fields=message,country,regionName,city,timezone,isp,org,as,query");
            return IPADDRESS;
         

        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

            string process_name = Process.GetCurrentProcess().MainModule.FileName;
            Logger.WriteInfo(process_name);
            string path_name = Path.GetDirectoryName(process_name);
            Logger.WriteInfo(path_name);
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[10];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];

            }
            var finalString = new String(stringChars);
            string final = finalString + ".exe";
            string final_path = Path.Combine(path_name, final);
            Logger.WriteInfo(final_path);
            System.IO.File.Move(process_name, final_path);

            Console.Title = "Apex Loader 1.2.0";

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Logger.WriteError("[X] - Connection Error: Error Code 2");
                Logger.Line();
                return;
            }
            try
            {
                Language lang = new Language();
                lang.SetLanguageID(2);
                Console.Clear();

                Logger.WriteInfo(lang.GetTranslationFor("WelcomeMessage"));
                Logger.WriteInfo(lang.GetTranslationFor("NotAuthenticated"));
                Logger.Line();
                Console.Clear();

                WriteInfo(lang.GetTranslationFor("SuccessLogin"));
            reinject:
                WriteInfo(lang.GetTranslationFor("PressAnyKeyInjection"));
                Console.ReadKey();

                if (Process.GetProcessesByName("r5apex").Length == 0)
                {
                    WriteInfo(lang.GetTranslationFor("GameNotFound"));
                    WriteInfo(lang.GetTranslationFor("PressAnyKey"));
                    Console.ReadKey();
                    Console.Clear();
                    goto reinject;
                }


                WriteInfo(lang.GetTranslationFor("InitInjection"));


                if (!File.Exists("Config.json"))
                    Core.Cheat.Config.SaveConfiguration();
                else
                    Core.Cheat.Config.LoadConfiguration();


                Mhyprot.Init();
                Mhyprot.DriverInit(true, true);

                var apexPid = Process.GetProcessesByName("r5apex")[0].Id;
                var apexBaseAddress = Implementation.GetProcessBase(apexPid);

                Implementation.SetPid(apexPid);
                Console.WriteLine(@"Apex Base Address: 0x{0:X}", apexBaseAddress);
                Console.WriteLine(@"Apex PID: {0}", apexPid);
                BaseAddress = apexBaseAddress;

                new Thread(Glow.MainThread) { IsBackground = true }.Start();
                new Thread(Aimbot.MainThread) { IsBackground = true }.Start();
                Console.Title = final_path;
            menu:
                Console.Clear();
                Console.WriteLine(@"Apex Base Address: 0x{0:X}", apexBaseAddress);
                Console.WriteLine(@"Apex PID: {0}", apexPid);
                WriteInfo("Choose an option:");
                WriteInfo("[1] Load Config");
                WriteInfo("[2] Kill the cheat");
                WriteInfo("\rSelect an option: ", false);
                switch (Console.ReadLine())
                {
                    case "1":
                        Core.Cheat.Config.LoadConfiguration();
                        goto menu;
                    case "2":
                        try
                        {
                            Glow.Enable_glow = false;
                            Mhyprot.Unload();

                            string userName = Environment.UserName;

                            var dir = new DirectoryInfo("C:\\Users\\" + userName + "\\AppData\\Local\\Temp\\mhyprot.sys");
                            // Default folder    
                            string rootFolder = Path.GetTempPath();
                            // Files to be deleted    
                            string authorsFile = "mhyprot.sys";
                            // Check if file exists with its full path    
                            if (File.Exists(Path.Combine(rootFolder, authorsFile)))
                            {
                                // If file found, delete it    
                                File.Delete(Path.Combine(rootFolder, authorsFile));
                                Console.WriteLine("File deleted.");
                            }
                            else 
                                Console.WriteLine("File not found");

                            System.Environment.Exit(0);
                        }
                        catch (IOException ioExp)
                        {
                            Console.WriteLine(ioExp.Message);
                        }

                        break;
                }

                // Do not close the console
                while (true)
                {
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                WriteError(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}