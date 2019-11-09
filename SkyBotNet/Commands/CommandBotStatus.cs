using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;

namespace SkyBotNet.Commands
{
    public class CommandBotStatus
    {
        [Command("hi")]
        public async Task Hi(CommandContext context)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"Hi {context.User.Username}",
                ImageUrl = ImageArr()
            };
            await context.RespondAsync(embed: embed);
        }

        string ImageArr()
        {
            Random random = new Random();
            string[] images = { "https://kotsobaka.com/wp-content/uploads/2018/05/24ff7159e7c8fe06f4210a1ebfba247bf7ada5d5ff6d27cd112bd73373e61520.jpg", "https://memepedia.ru/wp-content/uploads/2018/08/1476772041569.jpg", "https://i.v-s.mobi/2K8dVAkEHhtrGiy5ENCzMK6mxXtA7AJfM-5gk23AL-dQsh-gNOf37LS5v0VXTRUvtkwPXU3sujKnL1Q4DBYWuG1tq-UHQ.jpg" };
            return images[random.Next(0, 2)];
        }
    }
}
