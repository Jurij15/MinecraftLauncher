using Microsoft.UI.Xaml.Controls;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Managers
{
    public class CustomProfileDataManager
    {
        public static string RootProfilesDir = Settings.RootDir + "Profiles";

        public static string GetUsernameConfig(string GUID)
        {
            return RootProfilesDir + "/" + GUID + "/UsernameConfig";
        }

        public static string GetSubTextConfig(string GUID)
        {
            return RootProfilesDir + "/" + GUID + "/SubTextConfig";
        }

        public void InitProfiles()
        {
            Directory.CreateDirectory(RootProfilesDir);

            string guid = Guid.NewGuid().ToString();
            string dir = RootProfilesDir + "/" + guid;
            Directory.CreateDirectory(dir);

            using (StreamWriter sw = File.CreateText(dir + "/UsernameConfig"))
            {
                sw.WriteLine("Player");
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(dir + "/SubTextConfig"))
            {
                sw.WriteLine("A Minecraft Player");
                sw.Close();
            }

            Globals.Username = "Player";
            Globals.SubText = "A Minecraft Player";

            Settings.SaveLastUsedProfile(guid);
        }

        public void CreateNewProfile()
        {
            string dir = RootProfilesDir + "/" + Guid.NewGuid();
            Directory.CreateDirectory(dir);

            using (StreamWriter sw = File.CreateText(dir + "/UsernameConfig"))
            {
                sw.WriteLine("Player");
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(dir + "/SubTextConfig"))
            {
                sw.WriteLine("A Minecraft Player");
                sw.Close();
            }
        }

        public void GetLastUsedProfile()
        {
            string guid = File.ReadAllText(Settings.LastSelectedProfileIDConfig);

            Globals.Username = File.ReadAllText(GetUsernameConfig(guid));
            Globals.SubText = File.ReadAllText(GetSubTextConfig(guid));
        }

        public void SaveNewUsernameConfigToGuid(string guid)
        {
            File.Delete(GetUsernameConfig(guid));
            using (StreamWriter sw = File.CreateText(GetUsernameConfig(guid)))
            {
                sw.Write(Globals.Username);
                sw.Close();
            }
        }

        public void SaveNewSubTextConfigToGuid(string guid)
        {
            File.Delete(GetSubTextConfig(guid));
            using (StreamWriter sw = File.CreateText(GetSubTextConfig(guid)))
            {
                sw.Write(Globals.SubText);
                sw.Close();
            }
        }

        public List<string> GetAllGuids()
        {
            var list = new List<string>();
            foreach (var item in Directory.GetDirectories(RootProfilesDir))
            {
                list.Add(item);
            }

            return list;
        }

        public string GetUsernameByGuid(string Guid)
        {
            return File.ReadAllText(GetUsernameConfig(Guid));
        }

        public string GetSubTextByGuid(string Guid)
        {
            return File.ReadAllText(GetSubTextByGuid(Guid));
        }

        public void DeleteProfile(string guid)
        {
            Directory.Delete(RootProfilesDir + "/" + guid, true);
        }
    }
}
