using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataTasks;
using Windows.Graphics.Printing3D;

namespace MinecraftLauncherUniversal.Managers
{
    public class DatabaseManager
    {
        public static SQLiteConnection DatabaseConnection;

        public static async Task ConnectToDB()
        {
            DatabaseConnection = new SQLiteConnection(Globals.SQLiteConnectionPath);
            await DatabaseConnection.OpenAsync();
        }

        public async Task<bool> AddNewUser(string username, string SubText = "", string SkinPath = "")
        {
            bool RetVal = false;
            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
            {
                return false ;
            }

            try
            {
                string CreateTableSqlCommand = "CREATE TABLE IF NOT EXISTS Profile (ID STRING PRIMARY KEY AUTOINCREMENT, Username STRING, SubText STRING, ProfilePicture STRING)";
                SQLiteCommand CreateCommand = new SQLiteCommand(CreateTableSqlCommand, DatabaseConnection);
                await CreateCommand.ExecuteNonQueryAsync();

                string InsertSqlCommand = "INSERT INTO App (Username, SubText, ProfilePicture)" + " VALUES('" + username + "', '" + SubText + "', '" + "')";
                SQLiteCommand InsertCommand = new SQLiteCommand(InsertSqlCommand, DatabaseConnection);
                await InsertCommand.ExecuteNonQueryAsync();

                RetVal = true;
            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        public async Task<string> GetProfileUsernameFromID(string ID)
        {
            string RetVal = null;

            string Command = "SELECT Username FROM Profile WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, DatabaseConnection);
            SQLiteDataReader reader = (SQLiteDataReader)await SelectCommand.ExecuteReaderAsync();
            while (reader.Read())
            {
                RetVal = reader.GetString(0);
            }

            return RetVal;
        }

        public async Task<string> GetProfileSubTextFromID(string ID)
        {
            string RetVal = null;

            string Command = "SELECT SubText FROM Profile WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, DatabaseConnection);
            SQLiteDataReader reader = (SQLiteDataReader)await SelectCommand.ExecuteReaderAsync();
            while (reader.Read())
            {
                RetVal = reader.GetString(0);
            }

            return RetVal;
        }

        public async Task UpdateProfile(string ID, string Username, string SubText)
        {
            //tried using chatgpt for this, its good
            string command = "UPDATE Profile SET AppName = @Username, SubText = @SubText WHERE ID = @id";
            using (var cmd = new SQLiteCommand(command, DatabaseConnection))
            {
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@SubText", SubText);
                cmd.Parameters.AddWithValue("@ID", ID);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public static async Task RemoveProfileFromDatabaseByIDAsync(string ID)
        {
            string Command = "Delete FROM Profile WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, DatabaseConnection);
            await SelectCommand.ExecuteNonQueryAsync();
        }
    }
}
