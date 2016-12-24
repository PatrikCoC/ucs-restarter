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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(AsciiArt);
            Console.ResetColor();

            WriteLineInfo("Server Restarter loaded successfully.\n");
            WriteLineNote("UCS and Restarter executables must be in the same folder!");
            WriteLineGuide("Restart interval sample: 12:00:00 [hours:minutes:seconds] and can not be more than 24 hours!\n");
        }

        public static void WriteLineError(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[ERROR]  : " + value);
            Console.ResetColor();
        }

        public static void WriteLineInfo(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("[INFO]   : " + value);
            Console.ResetColor();
        }

        public static void WriteLineGuide(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[GUIDE]  : " + value);
            Console.ResetColor();
        }

        public static void WriteLineNote(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[NOTE]   : " + value);
            Console.ResetColor();
        }

        public static void WriteLineInput(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[INPUT]  : " + value);
            Console.ResetColor();
        }

        public static void DeleteLine()
        {
            Console.CursorLeft = 0;

            var emptyLine = new string(' ', Console.BufferWidth -1);
            Console.Write(emptyLine);

            Console.CursorLeft = 0;
        }
    }
}
