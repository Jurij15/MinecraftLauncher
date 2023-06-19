using MinecraftLauncherUniversal.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Core
{
    public class Profile : ProfileManager
    {
        private string _id;
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
            _Username = await GetProfileUsernameFromID(ID);
            _Subtext = await GetProfileSubTextFromID(ID);
        }

        public string GetUsername()
        {
            return _Username;
        }

        public string GetSubtext()
        {
            return _Subtext;
        }

        public async void Delete()
        {
            await RemoveProfileFromDatabaseByIDAsync(_id);
        }

        public async void Update(string Name = null, string SubText = "")
        {
            if (Name != null)
            {
                await UpdateProfileUsername(_id, Name);
            }
            if (SubText != null)
            {
                await UpdateProfileSubText(_id, SubText);
            }
        }
    }
}
