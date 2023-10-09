using CmlLib.Core;
using MinecraftLauncherUniversal.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tommy;

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

        public List<McMod> GetAllMods()
        {
            List<McMod> mods = new List<McMod>();

            foreach (var item in GetAllModFilesNames())
            {
                string path = Path.Combine(MinecraftPath.WindowsDefaultPath, "mods", item+".jar");
                string tomlPath = "META-INF/mods.toml";

                using (ZipArchive archive = ZipFile.OpenRead(path))
                {
                    ZipArchiveEntry entry = archive.GetEntry(tomlPath);

                    if (entry != null)
                    {
                        using (Stream stream = entry.Open())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                TomlTable table = TOML.Parse(reader);

                                McMod mod = new McMod();
                                mod.ModLicense = table["license"];

                                mod.ModID = table["modId"];
                                mod.ModVersion = table["version"];
                                mod.ModName = table["displayName"];
                                mod.ModAuthor = table["authors"];
                                mod.ModDescription = table["description"];

                                mods.Add(mod);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("File not found inside the JAR archive.");
                    }
                }
            }

            return mods;
        }
    }
}
