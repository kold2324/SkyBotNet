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

        static DiscordClient client;
        static CommandsNextModule commands;
        static InteractivityModule interactivity;

        static void Main(string[] args) => MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            GetConfig(out TOKEN, out Prefix);

            client = new DiscordClient(new DiscordConfiguration
            {
                Token = TOKEN,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            commands = client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = Prefix
            });

            interactivity = client.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = TimeoutBehaviour.Ignore,
                PaginationTimeout = TimeSpan.FromMinutes(5),
                Timeout = TimeSpan.FromMinutes(2)
            });

            //commands.RegisterCommands();

            await client.ConnectAsync();
            await Task.Delay(-1);
        }

        static void GetConfig(out string token, out string prefix)
        {
            string path = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\config.json";
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
