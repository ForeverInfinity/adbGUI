using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace adbGUI.Methods
{
    public static class CLI
    {
        private static readonly Encoding standardOutputEncoding = Encoding.UTF8;
        private static readonly Encoding standardErrorEncoding = Encoding.GetEncoding((int)Helper.GetConsoleOutputCP());

        static CLI()
        {
            Commandline = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "cmd",
                Arguments = "/K prompt $g ",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                StandardOutputEncoding = standardOutputEncoding,
                StandardErrorEncoding = standardErrorEncoding
            };

            Commandline.EnableRaisingEvents = true;

            Commandline.StartInfo = startInfo;

            Commandline.Start();
        }

        public static Process Commandline { get; }

        public static List<int> GetChildProcesses()
        {
            List<int> lst = new List<int>(4);
            string query =
                $"SELECT ProcessID FROM Win32_Process " +
                $"WHERE ParentProcessID={Commandline.Id} " +
                $"AND Name!='conhost.exe'";

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                using (ManagementObjectCollection results = searcher.Get())
                {
                    foreach (ManagementBaseObject mo in results)
                    {
                        if (mo["ProcessID"] is uint pid)
                        {
                            lst.Add((int)pid);
                        }
                    }
                }
            }
            return lst;
        }

        public static void KillChildProcesses()
        {
            foreach (int pid in GetChildProcesses())
            {
                Debug.WriteLine($"Killing {pid})");
                Process.GetProcessById(pid).Kill();
            }
        }

        public static void KillChildProcessesAsync()
        {
            Task.Run(() => { KillChildProcesses(); });
        }

        public static void KillChildProcessesWithShell()
        {
            string input = "taskkill /F ";

            foreach (int pid in GetChildProcesses())
            {
                input += $"/PID {pid} ";
            }

            if (input == "taskkill /F ") return;

            Process cmd = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "cmd",
                Arguments = "/c " + input,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            cmd.StartInfo = startInfo;

            Debug.WriteLine("Executing: cmd /c " + input);

            cmd.Start();
        }

        public static void KillAllAdbProcessesWithShell()
        {
            string input = "taskkill /F ";

            foreach (Process process in Process.GetProcessesByName("adb"))
            {
                input += $"/PID {process.Id} ";
            }

            if (input == "taskkill /F ") return;

            Process cmd = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "cmd",
                Arguments = "/c " + input,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            cmd.StartInfo = startInfo;

            Debug.WriteLine("Executing: cmd /c " + input);

            cmd.Start();

        }

        public static void Execute(string command)
        {
            Commandline.StandardInput.WriteLine(command);
        }

        public static string GetOutput(string fileName, string arguments)
        {
            Process cmd = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                StandardOutputEncoding = standardOutputEncoding,
                StandardErrorEncoding = standardErrorEncoding
            };

            cmd.EnableRaisingEvents = true;

            cmd.StartInfo = startInfo;

            cmd.Start();

            return cmd.StandardOutput.ReadToEnd();
        }

        public static void StopWithShell()
        {
            KillChildProcessesWithShell();
            Commandline.Kill();
        }
        private static class Helper
        {
            [DllImport("kernel32.dll")]
            public static extern uint GetConsoleOutputCP();
        }
    }
}
