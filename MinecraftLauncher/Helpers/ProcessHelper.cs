using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Helpers
{
    public class ProcessHelper
    {
        public static bool IsProcessRunning(string ProcessName)
        {
            bool result = false;

            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName == ProcessName && !process.ProcessName.Contains("MinecraftLauncher"))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
