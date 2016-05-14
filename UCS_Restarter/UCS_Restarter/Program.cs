using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

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
            //args = new string[]
            //{
            //    @"C:\Users\<Your_UserName\Desktop\UCSPath\FileName.exe"
            //};

            if (args.Length < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: You must provide at least 1 argument, for example <FileName>");
                Console.ResetColor();

                Environment.Exit(1);
            }

            if (!File.Exists(args[0]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: '{0}' does not exists.", args[0]);
                Console.ResetColor();

                Environment.Exit(1);
            }

            Console.Title = "Ultrapowa Clash Server Restarter";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ultrapowa Clash Server Restarter");
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
                        if (processes[i].ProcessName == "WerFault")
                        {
                            if (processes[i].MainWindowTitle.Contains("Republic"))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Detected that UCS has crashed. Restarting...");
                                processes[i].Kill();
                                Console.ResetColor();
                            }
                        }
                    }
                    UCSProcess.Refresh();

                    // The process will restart if its closed now.
                   if(UCSProcess.HasExited)
                   {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Restarting UCS because its not responding...");
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
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Restarting UCS...");
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exception occured while restarting UCS.");
                    Console.WriteLine(ex);
                    Console.ResetColor();
                }

                Thread.Sleep(100);
            }
        }
    }
}
