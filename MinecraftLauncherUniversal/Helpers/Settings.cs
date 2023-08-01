using MinecraftLauncherUniversal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncherUniversal.Interop;

namespace MinecraftLauncherUniversal.Helpers
{
    public class Settings
    {
        public static string RootDir = "Settings/";
        public static string ThemeConfig = RootDir + "ThemeConfig";
        public static string RecentBuilds = RootDir + "RecentsConfig";
        public static string MemoryInGB = RootDir + "AllocationMemoryInGBConfig";
        public static string Fullscreen = RootDir + "FullscreenConfig";

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

            using (StreamWriter sw = File.CreateText(MemoryInGB))
            {
                sw.Write(2);
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(Fullscreen))
            {
                sw.Write(true);
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

            if (!File.Exists(ThemeConfig))
            {
                MessageBox.Show("Config will be reset to defaults!", "Failed to find ThemeConfig");
                using (StreamWriter sw = File.CreateText(ThemeConfig))
                {
                    sw.Write(1);
                    sw.Close();
                }
            }
            else
            {
                string theme = File.ReadAllText(ThemeConfig);
                Globals.Theme = Convert.ToInt32(theme);
            }

            if (!File.Exists(LastSelectedProfileIDConfig))
            {
                MessageBox.Show("Config will be reset to defaults!", "Failed to find LastSelectedProfileIDConfig");
                using (StreamWriter sw = File.CreateText(LastSelectedProfileIDConfig))
                {
                    sw.Write(0);
                    sw.Close();
                }
            }
            else
            {
                string Profile = File.ReadAllText(LastSelectedProfileIDConfig);
                Globals.LastUsedProfileID = Profile;
            }

            if (!File.Exists(MemoryInGB))
            {
                MessageBox.Show("Config will be reset to defaults!", "Failed to find MemoryInGB");
                using (StreamWriter sw = File.CreateText(MemoryInGB))
                {
                    sw.Write(2);
                    sw.Close();
                }
            }
            else
            {
                string mem = File.ReadAllText(MemoryInGB);
                Globals.MemoryAmountInGB = Convert.ToInt32(mem);
            }

            if (!File.Exists(Fullscreen))
            {
                MessageBox.Show("Config will be reset to defaults!", "Failed to find Fullscreen");
                using (StreamWriter sw = File.CreateText(Fullscreen))
                {
                    sw.Write(true);
                    sw.Close();
                }
            }
            else
            {
                string fullscreen = File.ReadAllText(Fullscreen);
                Globals.ShouldGoFullscreen = Convert.ToBoolean(fullscreen);
            }

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

        public static void SaveMemoryAllocationInGB(int Amount)
        {
            File.Delete(MemoryInGB);
            using (StreamWriter sw = File.CreateText(MemoryInGB))
            {
                sw.Write(Amount);
                sw.Close();
            }
        }

        public static void SaveFullscreenConfig(bool NewValue)
        {
            File.Delete(Fullscreen);
            using (StreamWriter sw = File.CreateText(Fullscreen))
            {
                sw.Write(NewValue);
                sw.Close();
            }
        }
    }
}
