using MinecraftLauncherUniversal.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Managers
{
    public class ProfileManager : DatabaseManager
    {
        public async Task CreateDefaultPlayerProfile()
        {
            await AddNewUser("Player", "A Minecraft Player");
        }

        public async Task<List<string>> GetAllIDS()
        {
            var ids = new List<string>();

            string Command = "SELECT ID FROM Profile";
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, DatabaseConnection);
            SQLiteDataReader reader = (SQLiteDataReader)await SelectCommand.ExecuteReaderAsync();
            while (reader.Read())
            {
                ids.Add(Convert.ToString(reader["ID"]));
            }

            return ids;
        } 
    }
}
