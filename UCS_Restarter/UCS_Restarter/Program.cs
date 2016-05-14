using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using static System.Console;

namespace UCS_Restarter
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

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(
                    @"
 _   _  ____ ____        ____           _             _            
| | | |/ ___/ ___|      |  _ \ ___  ___| |_ __ _ _ __| |_ ___ _ __ 
| | | | |   \___ \ _____| |_) / _ \/ __| __/ _` | '__| __/ _ \ '__|
| |_| | |___ ___) |_____|  _ <  __/\__ \ || (_| | |  | ||  __/ |   
 \___/ \____|____/      |_| \_\___||___/\__\__,_|_|   \__\___|_|   
                  ");

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

            Console.Title = "Ultrapowa Clash Server Restarter";
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("[INFO]    : Server Restarter loaded successfully! Infos will be shown here.");
            Console.ResetColor();

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
                    var processes = Process.GetProcesses();
                    for (int i = 0; i < processes.Length; i++)
                    {
                        if (processes[i].ProcessName == "WerFault") // This code required to close the "Filename.exe has stopped working" window.
                        {
                            if (processes[i].MainWindowTitle.Contains("Republic")) // Change "Republic" if you use different filename!!!
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("[ERROR]   : SmartDetect has detected that UCS has stopped working / crashed. Restarting...");
                                processes[i].Kill();
                                Console.ResetColor();
                            }
                        }
                    }
                    UCSProcess.Refresh();

                    // The process will restart if its closed now.
                   if(UCSProcess.HasExited)
                   {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("[ERROR]   : SmartDetect has detected an issue with UCS. Restarting...");
                        //UCSProcess.Kill();
                        Console.ResetColor();
                        UCSProcess = Process.Start(args[0]);

                        StartTime = DateTime.Now;
                        RestartTime = StartTime.AddMinutes(30); 
                        RestartCount++;
                        continue;
                    }

                    if (DateTime.Now >= RestartTime)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("[INFO]  : Restarting UCS...");
                        UCSProcess.Kill();
                        UCSProcess = Process.Start(args[0]);

                        StartTime = DateTime.Now;
                        RestartTime = StartTime.AddMinutes(30);
                        RestartCount++;
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("[ERROR]   : Exception occured while restarting UCS.");
                    Console.WriteLine(ex);
                    Console.ResetColor();
                }

                Thread.Sleep(100);
            }
        }

        private static void WriteLine(string v)
        {
            throw new NotImplementedException();
        }
    }
}
