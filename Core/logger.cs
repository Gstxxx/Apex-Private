using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Private_Apex.Core
{

    public static class Logger
    {
        public static void SetTile(string title) => Console.Title = title;
        public static string ReadLine() => Console.ReadLine();
        public static void Blank() => Console.WriteLine();
        public static void Line() => Console.ReadLine();
        public static void WriteInfo(string text, bool WriteLine = true) => Write(text, "i", ConsoleColor.DarkBlue, WriteLine);
        public static void WriteSuccess(string text, bool WriteLine = true) => Write(text, "+", ConsoleColor.Green, WriteLine);
        public static void WriteError(string text, bool WriteLine = true) => Write(text, "x", ConsoleColor.Red, WriteLine);
        public static void WriteWarn(string text, bool WriteLine = true) => Write(text, "!", ConsoleColor.Yellow, WriteLine);
        static void Write(string text, string tag, ConsoleColor color, bool writeLine)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"({DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}) [");
            Console.ForegroundColor = color;
            Console.Write(tag);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            if (!writeLine)
                Console.Write(text);
            else
                Console.WriteLine(text);
        }
        static void WriteCentered(string text)
        {
            Console.CursorLeft = 25;
            Console.WriteLine(text);
        }
    }
}
