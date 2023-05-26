using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Media.Media3D;

namespace MinecraftLauncherUniversal.Helpers
{
    public class Settings
    {
        //THIS NEEDS A REWRITE FOR UWP
        public static string RootDir = "Settings/";
        public static string ThemeConfig = RootDir + "ThemeConfig";
        public static string UsernameConfig = RootDir + "UsernameConfig";
        public static string RecentBuilds = RootDir + "RecentsConfig";
        public static void CreateSettings()
        {
            Directory.CreateDirectory(RootDir);

            using (StreamWriter sw = File.CreateText(ThemeConfig))
            {
                //sw.Write(Wpf.Ui.Appearance.Theme.GetSystemTheme().ToString());
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(UsernameConfig))
            {
                sw.Write("Player");
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(RecentBuilds))
            {
                //sw.Write("OptiFine 1.17.1");
                sw.Close();
            }
        }

        public static void GetSettings() 
        {
            if (!Directory.Exists(RootDir))
            {
                CreateSettings();
            }

            string theme = File.ReadAllText(ThemeConfig);
            if (theme.Contains("dark", StringComparison.OrdinalIgnoreCase))
            {
                Globals.Theme = 0;
            }
            else
            {
                Globals.Theme = 1;
            }

            string Username = File.ReadAllText(UsernameConfig);
            Globals.Username = Username;

            foreach (var line in File.ReadAllLines(RecentBuilds))
            {
                Globals.Recents.Add(line);
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

        public static void SaveNewTheme()
        {
            File.Delete(ThemeConfig);
            using (StreamWriter sw = File.CreateText(ThemeConfig))
            {
                if (Globals.Theme == 0)
                {
                    sw.Write("Dark");
                }
                else if (Globals.Theme == 1)
                {
                    sw.Write("Light");
                }
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
    }
}
