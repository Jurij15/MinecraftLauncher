using MinecraftLauncherUniversal.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Core
{
    public class Profile : CustomProfileDataManager
    {
        private string _id; //guid
        private string _Username;
        private string _Subtext;

        public Profile(string ID)
        {
            Initialize(ID);
        }

        public Profile()
        {

        }

        public async void Initialize(string ID)
        {
            _Username = GetUsernameByGuid(ID);
            _Subtext = GetSubTextByGuid(ID);
        }

        public string GetUsername()
        {
            return _Username;
        }

        public string GetSubtext()
        {
            return _Subtext;
        }

        public void Delete()
        {
            DeleteProfile(_id);
        }

        public void UpdateUsername(string Name)
        {
            SaveNewUsernameConfigToGuid(_id);
        }

        public void UpdateSubText(string SubText)
        {
            SaveNewSubTextConfigToGuid(_id);
        }
    }
}
