using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using SkyBotNet.Settings;

namespace SkyBotNet
{
    class Program
    {
        static string TOKEN;
        static string Prefix;

        static void Main(string[] args)
        {

        }

        static void GetConfig(out string token, out string prefix)
        {
            string path = $@"{Path.GetFileName(Assembly.GetExecutingAssembly().Location)}\config.json";
            string json = "";

            using (StreamReader reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
            }

            var config = JsonConvert.DeserializeObject<Config>(json);

            token = config.Token;
            prefix = config.Prefix;
        }
    }
}
