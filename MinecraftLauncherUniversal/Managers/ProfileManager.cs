using MinecraftLauncherUniversal.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Managers
{
    public class ProfileManager
    {
        public static bool bIsSkinFilePresent()
        {
            bool RetVal = false;
            if (File.Exists(Settings.ProfileDir + "skin"))
            {
                RetVal = true;
            }
            else
            {
                RetVal = false;
            }

            return RetVal;
        }

        public static bool bIsProfileImagePresent()
        {
            bool RetVal = false;
            if (File.Exists(Settings.ProfileDir + "skin"))
            {
                RetVal = true;
            }
            else
            {
                RetVal = false;
            }

            return RetVal;
        }
    }
}
