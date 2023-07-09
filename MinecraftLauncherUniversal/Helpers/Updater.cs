using CmlLib.Core.VersionMetadata;
using Microsoft.UI.Xaml;
using MinecraftLauncherUniversal.Enums;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel.UserDataTasks;
using UpdateStatus = MinecraftLauncherUniversal.Enums.UpdateStatus;

namespace MinecraftLauncherUniversal.Helpers
{
    public class Updater
    {
        public static string GetLatestVersionStringFromGitHub()
        {
            string RetVal = string.Empty;

            if (File.Exists("Settings/VersionTemp"))
            {
                File.Delete("Settings/VersionTemp");
            }

            Uri uri = new Uri("https://raw.githubusercontent.com/Jurij15/MinecraftLauncher/master/docs/api/latestVersion.txt");
            WebClient wc = new WebClient();
            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync(uri))
                    {
                        using (var fs = new FileStream("Settings/VersionTemp", FileMode.OpenOrCreate))
                        {
                            s.Result.CopyTo(fs);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                //DialogService.ShowSimpleDialog("Error", "An error occured while checking for updates");
                throw;
            }

            RetVal = File.ReadAllText("Settings/VersionTemp");
            return RetVal;
        }

        public static bool bIsUpToDate()
        {
            bool retVal = false;
            if (GetLatestVersionStringFromGitHub() == Globals.VersionString)
            {
                retVal = true;
            }

            return retVal;
        }

        public static bool bIsPrerelease()
        {
            bool retVal = false;
            if (Globals.VersionString.Contains("PR") || Globals.VersionString.Contains("DEV") || Globals.VersionString.Contains("Beta", StringComparison.OrdinalIgnoreCase))
            {
                retVal = true;
            }

            return retVal;
        }

        public static UpdateStatus GetUpdateStatus()
        {
            UpdateStatus status = UpdateStatus.Fail;

            try
            {
                bool value = Updater.bIsUpToDate();
                if (value)
                {
                    status = UpdateStatus.UpToDateWell;
                }
                else if (Updater.bIsPrerelease())
                {
                    status = UpdateStatus.PreRelease;
                }
                else if (!value)
                {
                    status = UpdateStatus.UpdateAvailable;
                }
            }
            catch (Exception ex)
            {
                status = UpdateStatus.Fail;
                throw;
            }

            return status;
        }
    }
}
