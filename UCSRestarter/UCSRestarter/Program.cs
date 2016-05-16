using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace UCSRestarter
{
    public class Program
    {
        public static DateTime StartTime;
        public static DateTime RestartTime;
        public static Process UCSProcess;
        public static int RestartCount;

        public static void Main(string[] args)
        {
            //args = new string[] // Delete the trailing slashes to set UCS Path if you don't want to use CMD
            //{
            //    @"C:\Users\<UserName>\Desktop\UCS-Path\UCS-Filename.exe"
            //};

            WriteLineColor(
                    @"
 _   _  ____ ____        ____           _             _            
| | | |/ ___/ ___|      |  _ \ ___  ___| |_ __ _ _ __| |_ ___ _ __ 
| | | | |   \___ \ _____| |_) / _ \/ __| __/ _` | '__| __/ _ \ '__|
| |_| | |___ ___) |_____|  _ <  __/\__ \ || (_| | |  | ||  __/ |   
 \___/ \____|____/      |_| \_\___||___/\__\__,_|_|   \__\___|_|   
                  ", ConsoleColor.DarkRed);

            if (args.Length < 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("[ERROR]   : You must provide the .exe to use restarter with!");
                Console.ResetColor();

                Environment.Exit(1);
            }

            if (!File.Exists(args[0]))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("[ERROR]   : '{0}' does not exists.", args[0]);
                Console.ResetColor();

                Environment.Exit(1);
            }

            Console.Title = "UCS Restarter";
            WriteLineColor("[INFO]   : Server Restarter loaded successfully! Infos will be shown here.", ConsoleColor.DarkGreen);

            UCSProcess = Process.Start(args[0]);
            StartTime = DateTime.Now;
            RestartTime = StartTime.AddMinutes(30); // Restart after every 30 minutes.

            while (true)
            {
                var remainingTime = RestartTime - DateTime.Now;
                var title = string.Format("UCS Restarter - Time left: {0}, Restart count: {1}", remainingTime, RestartCount);
                Console.Title = title;

                try
                {
                    // TODO: Set "WerFault" according to Operating System language.
                    var processes = Process.GetProcessesByName("WerFault");
                    for (int i = 0; i < processes.Length; i++)
                    {
                        if (processes[i].MainWindowTitle.Contains(UCSProcess.MainModule.FileVersionInfo.FileDescription))
                        {
                            // When that WerFault.exe thing closes UCS as well closes. You get this one
                            WriteLineColor("[ERROR]  : SmartDetect has detected that UCS has stopped working / crashed. Restarting...", ConsoleColor.DarkRed);
                            processes[i].Kill(); 
                        }
                    }

                    UCSProcess.Refresh();

                    // The process will restart if its closed.
                    if (UCSProcess.HasExited)
                    {
                        WriteLineColor("[ERROR]  : SmartDetect has detected an issue with UCS. Restarting...", ConsoleColor.DarkRed);
                        Restart(args[0]);
                        continue;
                    }

                    if (DateTime.Now >= RestartTime)
                    {
                        WriteLineColor("[INFO]   : Restarting UCS...", ConsoleColor.DarkGreen);
                        Restart(args[0]);
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("[ERROR]   : Exception occurred while restarting UCS.");
                    Console.WriteLine(ex);
                    Console.ResetColor();
                }

                Thread.Sleep(100);
            }
        }

        public static void Restart(string path)
        {
            // Make sure it is not dead before killing it because we don't want to kill a dead process.
            if (!UCSProcess.HasExited)
                UCSProcess.Kill();

            UCSProcess = Process.Start(path);

            StartTime = DateTime.Now;
            RestartTime = StartTime.AddMinutes(30);
            RestartCount++;
        }

        public static void WriteLineColor(string value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}
