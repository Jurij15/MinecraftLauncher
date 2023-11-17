using CommunityToolkit.WinUI.UI.Behaviors;
using MinecraftLauncherUniversal.Interop;
using MineStatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx.Messaging;

namespace MinecraftLauncherUniversal.Helpers
{
    public class ServersHelper
    {
        public static MineStat GetServer(string ServerIP, string ServerPort, int Timeout = 4)
        {
            return new MineStat(ServerIP, (ushort)Convert.ToInt32(ServerPort), Timeout);
        }


        public static bool IsServerUp(MineStat MinestatClass)
        {
            bool RetVal = false;

            if (MinestatClass.ServerUp && !MinestatClass.Stripped_Motd.Contains("offline", StringComparison.OrdinalIgnoreCase))
            {
                RetVal = true;
            }

            return RetVal;
        }
    }
}
