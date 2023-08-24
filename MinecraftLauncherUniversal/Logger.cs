using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Audio;

namespace MinecraftLauncherUniversal
{
    public enum LogImportance
    {
        Info,
        Success,
        Error,
        Warning
    }
    public class Logger
    {
        public static void Log(string Source, string Message)
        {
            Serilog.Log.Information(Message);
        }
    }
}
