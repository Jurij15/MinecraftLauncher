using MinecraftLauncherUniversal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Helpers
{
    public class Settings
    {
        public static string RootDir = "Settings/";
        public static string ThemeConfig = RootDir + "ThemeConfig";
        public static string RecentBuilds = RootDir + "RecentsConfig";

        public static string ProfileDir = RootDir + "Profile/";
        public static string UsernameConfig = ProfileDir + "UsernameConfig";
        public static string SubTextConfig = ProfileDir + "SubText";

        public static string LastSelectedProfileIDConfig = RootDir + "LastID";
        public static void CreateSettings()
        {
            Directory.CreateDirectory(RootDir);
            Directory.CreateDirectory(ProfileDir);

            using (StreamWriter sw = File.CreateText(ThemeConfig))
            {
                sw.Write(1);
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(UsernameConfig))
            {
                sw.Write("Player");
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(SubTextConfig))
            {
                sw.Write("Minecraft Player");
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(RecentBuilds))
            {
                //sw.Write("OptiFine 1.17.1");
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(LastSelectedProfileIDConfig))
            {
                sw.Write(0);
                sw.Close();
            }
        }

        public static void GetSettings() 
        {
            if (!Directory.Exists(RootDir))
            {
                CreateSettings();
                Globals.bIsFirstTimeRun = true;
            }

            string theme = File.ReadAllText(ThemeConfig);
            Globals.Theme = Convert.ToInt32(theme);

            string Username = File.ReadAllText(UsernameConfig);
            Globals.Username = Username;

            string SubText = File.ReadAllText(SubTextConfig);
            Globals.SubText = SubText;

            string Profile = File.ReadAllText(LastSelectedProfileIDConfig);
            Globals.LastUsedProfileID = SubText;

            foreach (var item in File.ReadAllLines(RecentBuilds))
            {
                Globals.Recents.Add(item);
            }
        }    

        public static void ResetSettings()
        {
            Directory.Delete(RootDir, true);
        }

        public static void SaveNewUsername()
        {
            File.Delete(UsernameConfig);
            using (StreamWriter sw = File.CreateText(UsernameConfig))
            {
                sw.Write(Globals.Username);
                sw.Close();
            }
        }

        public static void SaveNewSubText()
        {
            File.Delete(SubTextConfig);
            using (StreamWriter sw = File.CreateText(SubTextConfig))
            {
                sw.Write(Globals.SubText);
                sw.Close();
            }
        }

        public static void SaveNewTheme()
        {
            File.Delete(ThemeConfig);
            using (StreamWriter sw = File.CreateText(ThemeConfig))
            {
                sw.Write(Globals.Theme.ToString());
                sw.Close();
            }
        }

        public static void SaveRecentBuild(string BuildName)
        {
            using (StreamWriter sw = File.AppendText(RecentBuilds))
            {
                sw.WriteLine(BuildName);
                sw.Close();
            }
        }

        public static void SaveLastUsedProfile(string ProfileID)
        {
            File.Delete(LastSelectedProfileIDConfig);
            using (StreamWriter sw = File.CreateText(LastSelectedProfileIDConfig))
            {
                sw.Write(Globals.LastUsedProfileID.ToString());
                sw.Close();
            }
        }
    }
}
