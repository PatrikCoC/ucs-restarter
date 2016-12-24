using System;
using System.IO;
using System.Threading;

namespace UCSRestarter
{
    public class Program
    {
        public static Restarter Restarter { get; set; }

        public static void Main(string[] args)
        {
            Console.Title = "UCS Restarter - Not Running";
            Console.Clear();
            ConsoleUtils.WriteAsciiArt();

            var interval = TimeSpan.FromMinutes(30);

            // Make sure we have at least 1 argument.
            if (args.Length < 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                ConsoleUtils.WriteLineInput("Filename of UCS: ");
                Console.ForegroundColor = ConsoleColor.DarkGray;

                var path = Console.ReadLine();
                args = new string[]
                {
                    path
                };
            }

            // Make sure the argument (file) provided exists.
            if (!File.Exists(args[0]))
            {
                ConsoleUtils.WriteLineError(string.Format("File '{0}' does not exists.", args[0]));
                ConsoleUtils.WriteLineInput("Press ENTER to retry...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

                Main(new string[0]);
                return;
            }

            // Make sure the argument (file) provided is an .exe file.
            if (Path.GetExtension(args[0]) != ".exe")
            {
                ConsoleUtils.WriteLineError(string.Format("File '{0}' is not a .exe!", args[0]));
                ConsoleUtils.WriteLineError("Press ENTER to retry...");

                while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

                Main(new string[0]);
                return;
            }

            while (true)
            {
                Start:
                var intervalStr = string.Empty;

                // If we dont have a restart interval provided.
                if (args.Length == 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    ConsoleUtils.WriteLineInput("Restart interval: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                    intervalStr = Console.ReadLine();
                    args = new string[]
                    {
                        args[0]
                    };
                }

                if (!TimeSpan.TryParse(intervalStr, out interval))
                {
                    ConsoleUtils.WriteLineError("Could not parse '" + intervalStr + "'.");
                    ConsoleUtils.WriteLineInput("Press ENTER to retry or SPACE to continue with the default 30 minutes interval...");

                    while (true)
                    {
                        var read = Console.ReadKey();
                        if (read.Key == ConsoleKey.Enter)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                ConsoleUtils.DeleteLine();
                                Console.CursorTop--;
                            }
                            Console.CursorTop++;
                            goto Start;
                        }
                        else if (read.Key == ConsoleKey.Spacebar)
                        {
                            interval = TimeSpan.FromMinutes(30);
                            goto Exit;
                        }
                    }
                }

                Exit:
                break;
            }

            Console.Title = "UCS Restarter";

            // Pass argument to the Restarter.
            Restarter = new Restarter(args[0]);
            Restarter.RestartInterval = interval;
            Restarter.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
