using CmlLib.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Managers
{
    public class ModsManager
    {
        public ModsManager() { }

        public string[] GetAllModFilesNames()
        {
            List<string> jars = new List<string>();    
            foreach (var mod in Directory.GetFiles(Path.Combine(MinecraftPath.WindowsDefaultPath, "mods")))
            {
                jars.Add(Path.GetFileNameWithoutExtension(mod));
            }

            return jars.ToArray();
        }
    }
}
