using System;

namespace UCSRestarter
{
    public static class ConsoleUtils
    {
        public const string AsciiArt =
            @"
... _   _  ____ ____        ____           _             _            
...| | | |/ ___/ ___|      |  _ \ ___  ___| |_ __ _ _ __| |_ ___ _ __ 
...| | | | |   \___ \ _____| |_) / _ \/ __| __/ _` | '__| __/ _ \ '__|
...| |_| | |___ ___) |_____|  _ <  __/\__ \ || (_| | |  | ||  __/ |   
....\___/ \____|____/      |_| \_\___||___/\__\__,_|_|   \__\___|_|   
                  ";

        public static void WriteAsciiArt()
        {
            Console.WriteLine(AsciiArt);
        }

        public static void WriteLineError(string value)
        {
            WritePrefix('-', ConsoleColor.DarkRed);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("error: ");
            Console.ResetColor();

            Console.WriteLine(value);
        }

        public static void WriteLineInfo(string value)
        {
            WritePrefix('*', ConsoleColor.DarkYellow);
            Console.WriteLine(value);
        }

        public static void WriteLineResult(string value)
        {
            WritePrefix('+', ConsoleColor.DarkGreen);
            Console.WriteLine(value);
        }

        private static void WritePrefix(char prefix, ConsoleColor color)
        {
            Console.ResetColor();
            Console.Write("[");

            Console.ForegroundColor = color;
            Console.Write(prefix);
            Console.ResetColor();

            Console.Write("] ");
        }
    }
}
