using MinecraftLauncherUniversal.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Managers
{
    public class ServersManager
    {
        public static string ServersRootDir = Globals.RootDir + "Servers\\";

        public async Task<string> AddServer(ServerJson Json)
        {
            if (!Directory.Exists(ServersRootDir))
            {
                Directory.CreateDirectory(ServersRootDir);
            }
            string guid = Guid.NewGuid().ToString();
            Json.GUID = guid;
            var json = JsonConvert.SerializeObject(Json);

            using (StreamWriter sw = File.CreateText(ServersRootDir+guid+".json"))
            {
                await sw.WriteAsync(json);
                sw.Close();
            }

            return guid;
        }

        public string[] GetAllServerGuids()
        {
            if (!Directory.Exists(ServersRootDir))
            {
                Directory.CreateDirectory(ServersRootDir);
            }
            var list = new List<string>();

            foreach (var item in Directory.GetFiles(ServersRootDir))
            {
                list.Add(Path.GetFileNameWithoutExtension(item));
            }

            return list.ToArray();
        }

        public ServerJson GetServerJson(string GUID)
        {
            var json = File.ReadAllText(ServersRootDir+GUID+".json");

            return JsonConvert.DeserializeObject<ServerJson>(json);
        }
    }
}
