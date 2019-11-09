using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;
using System.Security.Cryptography;

namespace SkyBotNet.Commands
{
    public class CommandBotStatus
    {
        public void Text(CommandContext context)
        {
            context.RespondAsync("Hi");
        }

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

        [Command("status")]
        public async Task Status(CommandContext context, string password)
        {
            string hash = HashPassword(password);
            await context.RespondAsync(hash);
        }

        string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
    }
}
