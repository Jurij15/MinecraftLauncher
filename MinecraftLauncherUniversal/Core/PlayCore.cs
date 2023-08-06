using CmlLib.Core.Auth;
using CmlLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Microsoft.UI.Xaml.Controls;

namespace MinecraftLauncherUniversal.Core
{
    public class PlayCore
    {
        private string _version;
        private int _memoryMB;
        private bool _bfullscreen;
        private string _uuid;
        private string _accessToken;

        public PlayCore(string VersionName, int MemoryInMB, bool LaunchInFullscreen, string UUID, string AccessToken)
        {
            _version = VersionName;
            _memoryMB = MemoryInMB;
            _bfullscreen = LaunchInFullscreen;
            _uuid = UUID;
            _accessToken = AccessToken;
        }

        private string _errorDuringLaunch;
        public string? GetLaunchErrors()
        {
            string RetVal = null;

            RetVal = _errorDuringLaunch;

            return RetVal;
        }

        private string _errorDuringDownload;
        public string? GetDwonloadErrors()
        {
            string RetVal = null;

            RetVal = _errorDuringDownload;

            return RetVal;
        }

        public async Task<bool> Launch()
        {
            bool RetVal = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = Globals.DownloadRateLimit;

            var path = new MinecraftPath(); // use default directory

            var launcher = new CMLauncher(path);

            var launchOption = new MLaunchOption();
            launchOption.MaximumRamMb = _memoryMB;
            launchOption.FullScreen = _bfullscreen;
            launchOption.GameLauncherName = "Minecraft Launcher Universal";
            if (!string.IsNullOrEmpty(_uuid) && !string.IsNullOrWhiteSpace(_uuid))
            {
                MSession session  = new MSession();
                session.AccessToken = _accessToken;
                session.UUID = _uuid;
                session.Username = Globals.Settings.Username;
                launchOption.Session = session;
            }
            if (!string.IsNullOrEmpty(_accessToken) && !string.IsNullOrWhiteSpace(_accessToken))
            {
                MSession session = new MSession();
                session.AccessToken = _accessToken;
                session.Username = Globals.Settings.Username;
                launchOption.Session = session;
            }
            else
            {
                launchOption.Session = MSession.GetOfflineSession(Globals.Settings.Username);
            }

            try
            {
                var process = await launcher.CreateProcessAsync(_version, launchOption);

                process.Start();

                RetVal = true; //launched no problem
            }
            catch (Exception ex)
            {
                RetVal = false;
                _errorDuringLaunch = ex.Message;
            }

            return RetVal;
        }

        public async Task Download(Action<int> ProgressChanged)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = Globals.DownloadRateLimit;

            var path = new MinecraftPath();

            var launcher = new CMLauncher(path);

            var launchOption = new MLaunchOption
            {
                MaximumRamMb = _memoryMB,
                Session = MSession.GetOfflineSession(Globals.Settings.Username),
                FullScreen = _bfullscreen
            };

            launcher.ProgressChanged += (s, e) =>
            {
                ProgressChanged(e.ProgressPercentage);
            };

            try
            {
                var process = await launcher.CreateProcessAsync(_version, launchOption);

                process.Dispose();
                //process.Start(); we do just not launch the process, simple
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error message: " + ex.Message, "An error occured");
            }
        }
    }
}
