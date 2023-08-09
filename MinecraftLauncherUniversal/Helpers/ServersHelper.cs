using MineStatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Helpers
{
    public class ServersHelper
    {
        public static bool IsServerOnline(string ServerIP, string ServerPort, int Timeout = 4)
        {
            bool RetVal = false;

            MineStat ms = new MineStat(ServerIP, (ushort)Convert.ToInt32(ServerPort), Timeout);
            if (ms.ServerUp && ms.Stripped_Motd == "Offline") //for aternos, as their motd will say the actuall status
            {
                RetVal = false;
            }
            else
            {
                RetVal = true;
            }

            return RetVal;
        }

        public bool IsServerOnline(string ServerIP, int ServerPort, int Timeout = 4)
        {
            bool RetVal = false;

            MineStat ms = new MineStat(ServerIP, (ushort)ServerPort, Timeout);
            if (ms.ServerUp && ms.Stripped_Motd == "Offline") //for aternos, as their motd will say the actuall status
            {
                RetVal = false;
            }
            else
            {
                RetVal = true;
            }

            return RetVal;
        }
    }
}
