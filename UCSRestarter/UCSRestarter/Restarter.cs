﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace UCSRestarter
{
    // The main restarter class.
    public class Restarter
    {
        public Restarter(string path)
        {
            RestartedTimes = new List<DateTime>();
            _path = path;
        }

        // Determines if the Restarter is running(returns _started).
        public bool Started
        {
            get
            {
                return _started;
            }
        }

        // Determines if the process has crashed.
        public bool HasCrashed
        {
            get
            {
                if (GetWerFaultProcess() != null)
                    return true;

                return false;
            }
        }

        // Number of times the application was restarted.  => RestartedTimes.Count
        //public int RestartCount { get; private set; }

        // List of DateTime the application was restarted.
        public List<DateTime> RestartedTimes { get; private set; }

        // DateTime of the next restart time.
        public DateTime NextRestart { get; private set; }

        // Thread that does the restarting stuff.
        private Thread _restarterThread;
        // Process of application to restart.
        private Process _process;
        // Path to .exe file.
        private string _path;
        // Determines if the Restarter is running.
        private bool _started;

        // Starts the Restarter.
        public void Start()
        {
            _started = true;
            _process = Process.Start(_path);

            NextRestart = DateTime.Now.AddMinutes(30);

            _restarterThread = new Thread(HandleRestarting);
            _restarterThread.Name = "Restarter Thread";
            _restarterThread.Start();
        }

        // Stops the Restarter.
        public void Stop()
        {
            if (_started)
            {
                _started = false;
                _restarterThread.Abort();
            }
            else throw new InvalidOperationException("Bro u did not even start the thing.");
        }

        // Handles the restarting of stuff by checking if it crashes and stuff.
        private void HandleRestarting()
        {
            try
            {
                while (true)
                {
                    var remaining = (NextRestart - DateTime.Now).ToString(@"hh\:mm\:ss\.fff");
                    var title = "UCS Restarter - Remaining: " + remaining + ", Count: " + RestartedTimes.Count;
                    Console.Title = title;

                    // Check if has crashed.
                    var hasCrashed = HasCrashed;
                    if (hasCrashed)
                    {
                        ConsoleUtils.WriteLineResult("detected that ucs has crashed\n\t-> restarting ucs");

                        // Kill WerFault.exe to cause UCS process to exit.
                        var werFault = GetWerFaultProcess();
                        werFault.Kill();

                        Restart();
                    }

                    // Check if we have NextRestart time has passed.
                    if (DateTime.Now >= NextRestart)
                    {
                        ConsoleUtils.WriteLineResult("waited 30 minutes\n\t-> restarting ucs");

                        Restart();
                    }

                    // Be sure to sleep because we don't want to kill the CPU.
                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException)
            {
                // We don't care about those types exceptions.
            }
        }

        private Process GetWerFaultProcess()
        {
            var processes = Process.GetProcessesByName("WerFault");
            for (int i = 0; i < processes.Length; i++)
            {
                var fileDescription = _process.MainModule.FileVersionInfo.FileDescription;
                if (processes[i].MainWindowTitle.Contains(fileDescription))
                    return processes[i];
            }
            return null;
        }

        private void Restart()
        {
            // Make sure it is not dead before killing it because we don't want to kill a dead process.
            if (!_process.HasExited)
                _process.Kill();

            _process = Process.Start(_path);
            RestartedTimes.Add(DateTime.Now);
            NextRestart = DateTime.Now.AddMinutes(30);
        }
    }
}
