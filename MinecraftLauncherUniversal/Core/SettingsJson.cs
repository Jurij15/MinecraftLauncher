using Microsoft.UI.Xaml;
using MinecraftLauncherUniversal.Interop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Core
{
    public class SettingsJson
    {
        //PROPERTIES
        public ElementTheme Theme = ElementTheme.Default;
        public bool Sound = false;
        public bool ShowImageBackgroundInPlayPage = true;

        public int MemoryAllocationInGB = 2;
        public bool Fullscreen = true;

        public string Username = "Player";
        public string CustomUUID;
        public string CustomAccessToken;

        //FUNCTIONS
        public static void SaveSettings()
        {
            var json = JsonConvert.SerializeObject(Globals.Settings);

            if (!Directory.Exists(Globals.RootDir))
            {
                Globals.bIsFirstTimeRun = true;
                Directory.CreateDirectory(Globals.RootDir);
            }

            using (var fileStream = new FileStream(Globals.SettingsFile, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.Write(json);
                }
            }
        }

        public static void LoadSettings()
        {
            try
            {
                if (File.Exists(Globals.SettingsFile))
                {
                    using (var fileStream = new FileStream(Globals.SettingsFile, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new StreamReader(fileStream))
                        {
                            string json = reader.ReadToEnd();
                            SettingsJson config = JsonConvert.DeserializeObject<SettingsJson>(json);
                            Globals.Settings = config;
                        }
                    }
                }
                else
                {
                    Globals.Settings = new SettingsJson();
                    SaveSettings();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error occured while loading settings: " + ex.Message, "Error");
            }
        }
    }
}
