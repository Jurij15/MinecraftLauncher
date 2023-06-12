using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace MinecraftLauncherUniversal.Managers
{
    public enum Backdrop
    {
        Acrylic,
        Mica
    }
    public class BackdropManager
    {
        public void ApplyBackdrop(Backdrop Backdrop)
        {
            switch (Backdrop)
            {
                case Backdrop.Acrylic:
                    DesktopAcrylicBackdrop desktopAcrylicBackdrop = new DesktopAcrylicBackdrop();
                    Window.Current.SystemBackdrop = desktopAcrylicBackdrop;
                    break; 
                case Backdrop.Mica:
                    break;
            }
        }

        public void ApplyMicaKind(MicaKind Kind)
        {
            Globals.MainMicaBackdrop.Kind = Kind;
        }
    }
}
