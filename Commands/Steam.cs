using Discord.Audio.Streams;
using Discord.Commands;
using DiscordBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Steam : ModuleBase<SocketCommandContext>
    {
        [Command("steam")]
        [Summary("Returns a steam game.")]
        public async Task SteamAsync([Summary("Game to search.")][Remainder] string game)
        {
            Console.WriteLine(game);
            var result = new SteamService().GetInfo(game);
            
            string x = result[0];

            await ReplyAsync(x);
        }
    }
}
