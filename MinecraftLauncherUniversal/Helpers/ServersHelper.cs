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
        public static bool IsServerOnline(string ServerIP, string ServerPort, int Timeout = 4)
        {
            bool RetVal = false;

            MineStat ms = new MineStat(ServerIP, (ushort)Convert.ToInt32(ServerPort), Timeout); //MessageBox.Show(ms.Stripped_Motd.Contains("Offline").ToString(), ServerIP); sometimes, this just returns false idk why
            if (ms.ServerUp && ms.Stripped_Motd.Contains("Offline")) //for aternos, as their motd will say the actuall status
            {
                return false;
            }
            else if (!ms.ServerUp)
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

            MineStat ms = new MineStat(ServerIP, (ushort)Convert.ToInt32(ServerPort), Timeout);
            if (ms.ServerUp && ms.Stripped_Motd.Contains("Offline")) //for aternos, as their motd will say the actuall status
            {
                RetVal = false;
            }
            else if (!ms.ServerUp)
            {
                RetVal = false;
            }
            else
            {
                RetVal = true;
            }

            return RetVal;
        }

        public static MineStat GetServer(string ServerIP, int ServerPort, int Timeout = 4)
        {
            return new MineStat(ServerIP, (ushort)ServerPort, Timeout);
        }

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
