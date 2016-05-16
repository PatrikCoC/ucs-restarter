using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace UCSRestarter
{
    public class Program
    {
        public static Restarter Restarter { get; set; }
        public static DateTime StartTime;
        public static DateTime RestartTime;
        public static Process UCSProcess;
        public static int RestartCount;

        public static void Main(string[] args)
        {
            // Set the path to UCS directly here instead of using command prompt.
            //args = new string[]
            //{
            //    @"path2ucs.exe"
            //};

            Console.Title = "UCS Restarter - Not Running";
            ConsoleUtils.WriteAsciiArt();

            // Make sure we have at least 1 argument.
            if (args.Length < 1)
            {
                ConsoleUtils.WriteLineError("you must at least provide 1 argument which points the .exe file");
                Environment.Exit(1);
            }

            // Make sure argument(file) provided exists.
            if (!File.Exists(args[0]))
            {
                ConsoleUtils.WriteLineError(string.Format("file at '{0}' does not exists", args[0]));
                Environment.Exit(1);
            }

            // Make sure argument(file) provided is an .exe file.
            if (Path.GetExtension(args[0]) != ".exe")
            {
                ConsoleUtils.WriteLineError(string.Format("file '{0}' is not a .exe file type", args[0]));
                Environment.Exit(1);
            }

            Console.Title = "UCS Restarter";

            // Pass argument to the Restarter.
            Restarter = new Restarter(args[0]);
            Restarter.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
