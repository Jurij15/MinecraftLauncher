using CmlLib.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MinecraftLauncher.Helpers
{
    public class OptifineDownloader
    {
        /// <summary>
        /// This will download optifine 1.17.1 and place the jar into the default folder
        /// </summary>
        public static Task DownloadOptifine()
        {
            string DownloadLocation = "Settings/OptiFineInstaller1.17.1.jar";

            if (File.Exists(DownloadLocation))
            {
                File.Delete(DownloadLocation);
            }

            WebClient downloader = new WebClient();
            downloader.DownloadFile(new Uri("https://optifine.net/downloadx?f=OptiFine_1.17.1_HD_U_H1.jar&x=d21bb6454f13ec3a58cca35c8ae2f702"), DownloadLocation);

            Process p = new Process();
            p.StartInfo.FileName = "java";
            p.StartInfo.Arguments = "-jar Settings/OptiFineInstaller1.17.1.jar";
            p.Start();

            //string output = p.StandardOutput.ReadToEnd();
            //MessageBox.Show(output);

            return Task.Delay(100);
        }
    }
}
