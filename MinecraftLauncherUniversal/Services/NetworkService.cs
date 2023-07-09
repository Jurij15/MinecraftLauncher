using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace MinecraftLauncherUniversal.Services
{
    public class NetworkService
    {
        public static bool GetIsConnectedToNetwork()
        {
            return NetworkInformation.GetInternetConnectionProfile()?.NetworkAdapter != null;
        }
    }
}
