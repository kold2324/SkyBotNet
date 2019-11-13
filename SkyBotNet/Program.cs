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
using SkyBotNet.Commands;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;

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

            commands.RegisterCommands<CommandBotStatus>();
            commands.SetHelpFormatter<HelpFormatter>();

            client.Ready += Client_Ready;
            client.GuildAvailable += Client_GuildAvailable;
            client.ClientErrored += Client_ClientError;
            client.GuildMemberAdded += Client_GuildMemberAdded;
            client.GuildMemberRemoved += Client_GuildMemberRemoved;

            commands.CommandExecuted += Commands_CommandExecuted;
            commands.CommandErrored += Commands_CommandErrored;

            await client.ConnectAsync();
            await Task.Delay(-1);

            CommandBotStatus commandBot = new CommandBotStatus();
        }

        private static Task Client_GuildMemberRemoved(GuildMemberRemoveEventArgs e)
        {
            //644283897609322497
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"{e.Member.Username} left our server {e.Member.JoinedAt.ToString("hh:mm:ss")}",
                Color = DiscordColor.Azure
            };
            var channel = e.Guild.GetChannel(644283897609322497);
            channel.SendMessageAsync(embed: embed);

            return Task.CompletedTask;
        }

        private static Task Client_GuildMemberAdded(GuildMemberAddEventArgs e)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"Welcome {e.Member.Username} to our server {e.Member.JoinedAt.ToString("hh:mm:ss")}",
                Description = $"Читайте правила {e.Guild.GetChannel(638060254248304641).Mention}",
                Color = DiscordColor.Azure
            };
            var channel = e.Guild.GetChannel(637999573310242838);

            channel.SendMessageAsync(embed: embed);

            return Task.CompletedTask;
        }

        private static Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            // let's log the name of the command and user
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, "ExampleBot", $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'", DateTime.Now);
            e.Context.Member.JoinedAt.ToString();

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        private static async Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            // let's log the error details
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, "ExampleBot", $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            // let's check if the error is a result of lack
            // of required permissions
            if (e.Exception is ChecksFailedException ex)
            {
                // yes, the user lacks required permissions, 
                // let them know

                var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

                // let's wrap the response into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Access denied",
                    Description = $"{emoji} You do not have the permissions required to execute this command.",
                    Color = new DiscordColor(0xFF0000) // red
                };
                await e.Context.RespondAsync("", embed: embed);
            }
        }

        private static Task Client_Ready(ReadyEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "ExampleBot", "Client is ready to process events.", DateTime.Now);

            return Task.CompletedTask;
        }

        private static Task Client_GuildAvailable(GuildCreateEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "ExampleBot", $"Guild available: {e.Guild.Name}", DateTime.Now);

            return Task.CompletedTask;
        }

        private static Task Client_ClientError(ClientErrorEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Error, "SkyBot", $"Exception occured: {e.Exception.GetType()}: {e.Exception.Message}", DateTime.Now);

            return Task.CompletedTask;
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
