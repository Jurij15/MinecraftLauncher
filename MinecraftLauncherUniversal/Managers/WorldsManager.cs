using CmlLib.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Managers
{
    public class WorldsManager
    {
        //STRINGS
        public static string RootMinecraftWorldsDir = MinecraftPath.GetOSDefaultPath()+"\\saves\\";
        //STATIC LISTS
        public static List<string> WorldsPaths = new List<string>();

        public WorldsManager()
        {
        }

        public void LoadAllPaths()
        {
            foreach (var dir in Directory.GetDirectories(RootMinecraftWorldsDir))
            {
                if (dir == null)
                {
                    continue;
                }
                else
                {
                    WorldsPaths.Add(dir);
                }
            }
        }
    }
}
