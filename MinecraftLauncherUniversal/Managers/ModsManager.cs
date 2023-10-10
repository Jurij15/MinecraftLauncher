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

                                mod.ModAuthors = table["authors"];
                                mod.ModLicense = table["license"];
                                mod.ModDisplayURL = table["displayURL"];
                                mod.ModIssueTrackerUrl = table["issueTrackerURL"];

                                TomlArray modstable = table["mods"].AsArray;
                                foreach (var moditem in modstable)
                                {
                                    if (moditem.GetType() == typeof(TomlTable))
                                    {
                                        TomlTable modtable = (TomlTable)moditem;

                                        mod.ModID = modtable["modId"];
                                        mod.ModVersion = modtable["version"];
                                        mod.ModName = modtable["displayName"];
                                        mod.ModDescription = modtable["description"];

                                        if (mod.ModAuthors == null) //authors are in the mods table
                                        {
                                            mod.ModAuthors = modtable["authors"];
                                        }
                                    }
                                }

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
